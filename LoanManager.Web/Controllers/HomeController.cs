using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LoanManager.Service;
using LoanManager.Web.Models;
using System;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace LoanManager.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly INotificationService _notificationService;
        public HomeController(IUserService userService, IRoleService roleService, INotificationService notificationService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._notificationService = notificationService;
        }




        public IActionResult Index()
        {
            //if (RoleId() == Constants.AdminRole)
            //{
            //    return Redirect("~/Dashboard/");
            //}
             
            HomeViewModel _homeVM = new HomeViewModel();
            _homeVM.Users = _userService.GetAll().Count();
            _homeVM.Roles = _roleService.GetAll().Count();
            _homeVM.Table1 = 32;
            _homeVM.Table2 = 3454;
            _homeVM.Table3 = 4423;
            _homeVM.Table4 = 4975;
            _homeVM.Table5 = 9222;
            _homeVM.Table6 = 1002;

            return View(_homeVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl, string currentCulture = "en-US")
        {
            returnUrl = returnUrl.Replace(currentCulture, culture).ToString();
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        //dispose notifications
        public IActionResult Noti(string uId = "")
        {
            int user = Env.GetUserInfo("Id", User.Claims).ToInt32();
            var nofification = _notificationService.GetAll().FirstOrDefault(i => i.UniqueId == uId);
            nofification.IsRead = true;
            nofification.ModifiedBy = user;
            nofification.DateModified = DateTime.Now;
            _notificationService.Update(nofification);
            return Redirect(nofification.ProcessToUrl);
        }


        public JsonResult DonutChartData()
        {
            List<Dunut> _DunutList = new List<Dunut>();

            _DunutList.Add(new Dunut { value = 10, color = "#FF0000", highlight = "#FF0000", label = "Chrome" });
            _DunutList.Add(new Dunut { value = 32, color = "#800000", highlight = "#800000", label = "IE" });
            _DunutList.Add(new Dunut { value = 43, color = "#808000", highlight = "#808000", label = "Edge" });
            _DunutList.Add(new Dunut { value = 55, color = "#800080", highlight = "#800080", label = "Firefox" });
            _DunutList.Add(new Dunut { value = 33, color = "#0000FF", highlight = "#0000FF", label = "Opera" });

            return Json(_DunutList);
        }



        //line chart and area chart binding 
        public JsonResult LineChartData()//make a copy of this if you bind barchart or area charts
        {
            Chart _chart = new Chart();
            _chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            var random = new Random();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                fillColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                strokeColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointStrokeColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointHighlightFill = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointHighlightStroke = String.Format("#{0:X6}", random.Next(0x1000000)),
            });
            _dataSet.Add(new Datasets()
            {
                label = "Last Year",
                data = new int[] { 65, 59, 80, 81, 56, 55, 40, 55, 66, 77, 88, 34 },
                fillColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                strokeColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointStrokeColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointHighlightFill = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointHighlightStroke = String.Format("#{0:X6}", random.Next(0x1000000)),
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


