using LoanManager.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using LoanManager.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace LoanManager.Web
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService; 
        public NotificationViewComponent(INotificationService notificationService)
        {
            this._notificationService = notificationService; 
        }

        public IViewComponentResult Invoke(IEnumerable<Claim> Claim)
        {
            int user = Env.GetUserInfo("Id", Claim).ToInt32();
            var notify = _notificationService.GetAll().Where(i => i.ToUserId == user && i.IsRead == null).ToArray();

            return View(notify);
        }
         

    }
}

