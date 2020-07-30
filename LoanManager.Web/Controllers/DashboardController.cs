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
    public class DashboardController : BaseController
    { 
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly IAppSettingService _appSettingService;
        private readonly IGeneralSettingService _generalSettingService;
        private readonly INotificationService _notificationService;
        private readonly IMenuPermissionService _menuPermissionService;
        private readonly IRoleUserService _roleUserService;
        private readonly ILoanService _loanService;
        private readonly ILoanManService _loanManService;

        public DashboardController(IUserService userService,IRoleService roleService,IMenuService menuService,IAppSettingService appSettingService,IGeneralSettingService generalSettingService,INotificationService notificationService,IMenuPermissionService menuPermissionService,IRoleUserService roleUserService,ILoanService loanService,ILoanManService loanManService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._menuService = menuService;
            this._appSettingService = appSettingService;
            this._generalSettingService = generalSettingService;
            this._notificationService = notificationService;
            this._menuPermissionService = menuPermissionService;
            this._roleUserService = roleUserService;
            this._loanService = loanService;
            this._loanManService = loanManService;
            
        }
        public IActionResult Index()
        {
            DashboardViewModel homeVM = new DashboardViewModel();
            homeVM.User = _userService.GetAll().Count();
            homeVM.Role = _roleService.GetAll().Count();
            homeVM.Menu = _menuService.GetAll().Count();
            homeVM.AppSetting = _appSettingService.GetAll().Count();
            homeVM.GeneralSetting = _generalSettingService.GetAll().Count();
            homeVM.Notification = _notificationService.GetAll().Count();
            homeVM.MenuPermission = _menuPermissionService.GetAll().Count();
            homeVM.RoleUser = _roleUserService.GetAll().Sum(i=>i.Id);

            return View(homeVM);
        }

        public JsonResult LineDuleChart(int lastDay = 9) 
        { 
            Chart _chart = new Chart();
            var lastDays = DateTime.Now.Date.AddDays(-lastDay);
            var loan = _loanService.GetAll().Where(i => i.DateAdded.Date >= lastDays).Select(i => i.DateAdded).ToArray();
            var loanMan = _loanManService.GetAll().Where(i => i.DateAdded.Date >= lastDays).Select(i => i.DateAdded).ToArray();

 
            string[] arrLabels = new string[lastDay];
            int[] loanData = new int[lastDay];
            int[] loanManData = new int[lastDay];


            for (int i = 0; i < lastDay; i++)
            {
                var dateDynamic = DateTime.Now.Date.AddDays(-i);
                int year = dateDynamic.Year;
                int month = dateDynamic.Month;
                int day = dateDynamic.Day;
                DateTime newDate = new DateTime(year, month, day);
                arrLabels[i] = newDate.ToString("yyyy/MMM/dd");
                var hasloan = loan.Where(j => j.Date == newDate.Date);
                if (hasloan.Count() > 0) 
                    loanData[i] = hasloan.Count(); 
                else 
                    loanData[i] = 0;

                var hasloanMan = loanMan.Where(j => j.Date == newDate.Date);
                if (hasloanMan.Count() > 0) 
                    loanManData[i] = hasloanMan.Count(); 
                else 
                    loanManData[i] = 0;


            }
            _chart.labels = arrLabels;

            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            var random = new Random(); 
            _dataSet.Add(new Datasets()
            {
                label = "Loan",
                data = loanData,
                fillColor = String.Format("#{0:X6}", random.Next(0x1000000)), strokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointStrokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightFill = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightStroke = String.Format("#{0:X6}", random.Next(0x1000000))
            });
            _dataSet.Add(new Datasets()
            {
                label = "LoanMan",
                data = loanManData,
                fillColor = String.Format("#{0:X6}", random.Next(0x1000000)), strokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointStrokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightFill = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightStroke = String.Format("#{0:X6}", random.Next(0x1000000))
            });
 
            _chart.datasets = _dataSet;
            return Json(_chart);
        }
        public JsonResult LineChart2(int lastDay = 9) 
        { 
            Chart _chart = new Chart();
            var lastDays = DateTime.Now.Date.AddDays(-lastDay);
            var loan = _loanService.GetAll().Where(i => i.DateAdded.Date >= lastDays).Select(i => i.DateAdded).ToArray();

 
            string[] arrLabels = new string[lastDay];
            int[] loanData = new int[lastDay];


            for (int i = 0; i < lastDay; i++)
            {
                var dateDynamic = DateTime.Now.Date.AddDays(-i);
                int year = dateDynamic.Year;
                int month = dateDynamic.Month;
                int day = dateDynamic.Day;
                DateTime newDate = new DateTime(year, month, day);
                arrLabels[i] = newDate.ToString("yyyy/MMM/dd");
                var hasloan = loan.Where(j => j.Date == newDate.Date);
                if (hasloan.Count() > 0) 
                    loanData[i] = hasloan.Count(); 
                else 
                    loanData[i] = 0;


            }
            _chart.labels = arrLabels;

            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            var random = new Random(); 
            _dataSet.Add(new Datasets()
            {
                label = "Loan",
                data = loanData,
                fillColor = String.Format("#{0:X6}", random.Next(0x1000000)), strokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointColor = String.Format("#{0:X6}", random.Next(0x1000000)),
                pointStrokeColor = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightFill = String.Format("#{0:X6}", random.Next(0x1000000)), pointHighlightStroke = String.Format("#{0:X6}", random.Next(0x1000000))
            });
 
            _chart.datasets = _dataSet;
            return Json(_chart);
        }


    }
}
