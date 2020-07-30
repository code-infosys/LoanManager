using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LoanManager.Service; 
using LoanManager.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LoanManager.Web.Controllers
{
    public class AppSettingController : BaseController
    {
        private readonly IAppSettingService _appSettingService;

        public AppSettingController(IAppSettingService appSettingService)
        {
            this._appSettingService = appSettingService;

        }

        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult GetGrid([FromBody]DTParameters param)
        {
             return Json(_appSettingService.GetGrid(param)); 
        }
 
        [HttpGet]
        public PartialViewResult Create()
        {
            AppSetting model = new AppSetting();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Create([FromForm]AppSetting model,IFormFile logo, string logo11,IFormFile loginPageBackground, string loginPageBackground12)
        {
            AlertBack alert = new AlertBack();
            try
            {
                if (ModelState.IsValid)
                {
                    if (logo != null)
                    { 
                        ModelState.Clear();
                        model.Logo = Env.GetUploadedFilePath(logo).Result; 
                    }
                    else
                    {
                        model.Logo = logo11;
                    }
if (loginPageBackground != null)
                    { 
                        ModelState.Clear();
                        model.LoginPageBackground = Env.GetUploadedFilePath(loginPageBackground).Result; 
                    }
                    else
                    {
                        model.LoginPageBackground = loginPageBackground12;
                    }

                    _appSettingService.Insert(model);
                    alert.Status = "success";
                    alert.Message = "Register Successfully";
                }
                else
                {
                    alert.Status = "warning";
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            alert.Message += err.ErrorMessage + "<br/>";
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                alert.Status = "error";
                alert.Message = ex.Message; 
            }

            return Json(alert); 
        }
 
         
        public PartialViewResult Edit(int id)
        { 
            AppSetting ObjAppSetting = _appSettingService.Get(id);

            return PartialView(ObjAppSetting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppSetting model,IFormFile logo, string logo11,IFormFile loginPageBackground, string loginPageBackground12)
        {
            AlertBack alert = new AlertBack(); 
            try
            {
                if (ModelState.IsValid)
                {
                    if (logo != null)
                    { 
                        ModelState.Clear();
                        model.Logo = Env.GetUploadedFilePath(logo).Result; 
                    }
                    else
                    {
                        model.Logo = logo11;
                    }
if (loginPageBackground != null)
                    { 
                        ModelState.Clear();
                        model.LoginPageBackground = Env.GetUploadedFilePath(loginPageBackground).Result; 
                    }
                    else
                    {
                        model.LoginPageBackground = loginPageBackground12;
                    }

                   _appSettingService.Update(model);
                    alert.Status = "success";
                    alert.Message = "Register Successfully";
                }
                else
                {
                    alert.Status = "warning";
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            alert.Message += err.ErrorMessage + "<br/>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                alert.Status = "error";
                alert.Message = ex.Message; 
            }
             
            return Json(alert); 
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                AppSetting ObjAppSetting = _appSettingService.Get(id); 
               _appSettingService.Delete(ObjAppSetting);

                sb.Append("Sumitted");
                return Content(sb.ToString());

            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());
        }

    }
}

