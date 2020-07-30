using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManager.Web
{
    [Authorize]
    public class BaseController : Controller
    {
        public int RoleId()
        {
            return Env.GetUserInfo("RoleId", User.Claims).ToInt32();
        }
        public int UserId()
        {
            return Env.GetUserInfo("Id", User.Claims).ToInt32();
        }
        public string RoleName()
        {
            return Env.GetUserInfo("Role", User.Claims);
        }
        public string UserName()
        {
            return Env.GetUserInfo("UserName", User.Claims);
        }
    }
     
     
}
