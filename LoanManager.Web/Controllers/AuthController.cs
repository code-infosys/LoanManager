using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LoanManager.Models;
using LoanManager.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace LoanManager.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthLoginService _authLoginService;
        private readonly IConfiguration _configuration;
        private readonly IRoleUserService _roleUserService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IAppSettingService _appSettingService;
        private readonly IGeneralSettingService _generalSettingService;
       
        public AuthController(IAuthLoginService authLoginService, IConfiguration configuration, IRoleUserService roleUserService, IRoleService roleService, IUserService userService, IAppSettingService appSettingService, IGeneralSettingService generalSettingService)
        {
            this._authLoginService = authLoginService;
            this._configuration = configuration;
            this._roleUserService = roleUserService;
            this._roleService = roleService;
            this._userService = userService;
            this._appSettingService = appSettingService;
            this._generalSettingService = generalSettingService;
        }
         


        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            var setting = _appSettingService.GetAll().Select(i => new { i.LoginPageBackground });
            ViewBag.LoginPageBackground = setting.Select(i => i.LoginPageBackground).FirstOrDefault();

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (isValid, user) = await _authLoginService.ValidateUserCredentialsAsync(model.UserName);
                string dbPassword = user.Password.ToString();

                bool CheckPassword = false;
                if (SecurePwdHasherHelper.Verify(model.Password, dbPassword))
                {
                    CheckPassword = true;
                }

                if (isValid && CheckPassword)
                {
                    var roleuser = _roleUserService.GetAll().Where(i => i.UserId == user.Id).Select(i => i.RoleId).FirstOrDefault();
                    var role = _roleService.GetAll().Where(i => i.Id == roleuser).Select(i => i.RoleName).FirstOrDefault();
                    var setting = _appSettingService.GetAll().FirstOrDefault();
                    if (roleuser > 0 && role != null)
                    {
                        await LoginAsync(user, roleuser, role, setting);
                        if (IsUrlValid(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("RoleNotAssigned", "Role is Not Assigned.");
                    }
                }
                else
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
                }
            }

            return View(model);
        }

        private static bool IsUrlValid(string returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl)
                   && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative);
        }

        private async Task LoginAsync(User user, int roleid, string rolename, AppSetting s)
        {
            var properties = new AuthenticationProperties
            {
                //AllowRefresh = false,
                //IsPersistent = true,
                //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10)
            };

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, rolename),
                    new Claim(ClaimTypes.Rsa, roleid.ToString()),
                    new Claim(ClaimTypes.Webpage, "AppName="+s.AppName+"#AppShortName="+s.AppShortName +"#AppShortName="+s.AppShortName+"#AppVersion="+s.AppVersion+"#FooterText="+s.FooterText+"#Skin="+s.Skin+"#IsToggleSidebar="+s.IsToggleSidebar+"#IsToggleRightSidebar="+s.IsToggleRightSidebar+"#IsFixedLayout="+s.IsFixedLayout+"#IsBoxedLayout="+s.IsBoxedLayout+"#Logo="+s.Logo+"#LoginPageBackground="+s.LoginPageBackground+"#TimeZone="+s.TimeZone)

                    //new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString("o"), ClaimValueTypes.DateTime)
                };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal, properties);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!_configuration.GetValue<bool>("Account:ShowLogoutPrompt"))
            {
                return await Logout();
            }

            return View();
        }


        public IActionResult Cancel(string returnUrl)
        {
            if (IsUrlValid(returnUrl))
            {
                return Redirect(returnUrl);
            } 
            return RedirectToAction("login", "Auth");
        }

        [HttpPost]
        [Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("login", "Auth");
        }

        [Route("unauthorized")]
        public IActionResult unauthorized()
        {
            return View();
        }

        [Route("forget")]
        [HttpPost]
        public async Task<JsonResult> Forget(string Username)
        {
            try
            {
                var (isValid, user) = await _authLoginService.ValidateUserCredentialsAsync(Username);

                if (isValid)
                {
                    //update user data
                    User model = new User();
                    model = user;

                    model.ChangePasswordCode = Env.Encrypt(user.Email);
                    model.IsActive = false;
                    _userService.Update(model);
                    //end update user data

                    var GetSiteRoot = $"{this.Request.Scheme}://{this.Request.Host}";
                    string subject = "My Password";
                    string body = "Click here to reset your password<br> <a href='" + GetSiteRoot + "/auth/forgot/?id=" + user.ChangePasswordCode + "'>Click Here to reset</a> ";
                    var gs = _generalSettingService.GetAll().Where(i => i.SettingGroup == "smtp").ToArray();

                    Env.SendEmail(user.Email, subject, body, 
                        gs.FirstOrDefault(i=>i.SettingKey== "SmtpUsername").SettingValue,
                        gs.FirstOrDefault(i => i.SettingKey == "SmtpPassword").SettingValue,
                        gs.FirstOrDefault(i => i.SettingKey == "SmtpPort").SettingValue,
                        gs.FirstOrDefault(i => i.SettingKey == "SmptServer").SettingValue);
                     
                    return Json(new { status = "success", message = "Successfully Please check your email." });
                }
                else
                { 
                    return Json(new { status = "warning", message = "Entered Username does not exists." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }



        [Route("forgot")]
        public IActionResult Forgot(string id)
        {
            try
            {
                string dusername = Env.Decrypt(id);
                var user = _userService.GetAll().FirstOrDefault(i => i.IsActive == false && i.Email == dusername);

                if (user != null)
                {
                    ViewBag.Valid = true;
                    ViewBag.id = id;
                }
                else
                    ViewBag.Valid = false;
            }
            catch (Exception)
            {
                ViewBag.Valid = false;
            } 
            return View();
        }

        [Route("forgot")]
        [HttpPost]
        public JsonResult Forgot(string id, string Password, string ConfirmPassword)
        { 
            if (Password == ConfirmPassword)
            {
                string dusername = Env.Decrypt(id);
                var user = _userService.GetAll().FirstOrDefault(i => i.IsActive == false && i.Email == dusername);

                if (user != null)
                {
                    user.Password = SecurePwdHasherHelper.Hash(Password);
                    user.ChangePasswordCode ="";
                    user.IsActive = true;
                    _userService.Update(user); 
                    return Json(new { status = "success", message = "Successfully Reset now you may login" });
                }
                else
                { 
                    return Json(new { status = "warning", message = "Please re-send request for forget password." });
                }
            }
            else
            { 
                return Json(new { status = "warning", message = "Password are not matached." });
            } 
        }


        [Route("register")]
        public IActionResult Register()
        {
            ViewBag.FbAppId = _generalSettingService.GetAll().FirstOrDefault(i => i.SettingKey == "FbAppId").SettingValue;
            ViewBag.GoogleKeyForLogin = _generalSettingService.GetAll().FirstOrDefault(i => i.SettingKey == "GoogleKeyForLogin").SettingValue;
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromForm]User model, string NewPasswordConfirm)
        {
            string msg = string.Empty;
            string msgstatus = string.Empty;

            if (ModelState.IsValid)
            { 
                if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(NewPasswordConfirm))
                {
                    if (model.Password == NewPasswordConfirm)
                    {
                        model.Password = SecurePwdHasherHelper.Hash(model.Password);
                        _userService.Insert(model); 
                        return Json(new { status = "success", message = "Register Successfully" });
                    }
                    else
                    {
                        return Json(new { status = "warning", message = "Confirm password mismatch" });
                    }
                }
                else
                {
                    return Json(new { status = "warning", message = "Fill Password Fields" });
                } 
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var key in this.ViewData.ModelState.Keys)
                {
                    foreach (var err in this.ViewData.ModelState[key].Errors)
                    {
                        sb.Append(err.ErrorMessage + "<br/>");
                    }
                }

                return Json(new { status = "warning", message = sb.ToString() });
            } 
        }

    }
}

