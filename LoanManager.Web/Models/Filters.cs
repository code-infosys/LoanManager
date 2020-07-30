using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System; 
using System.Linq; 
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using LoanManager.Service; 

namespace LoanManager.Web
{
    public class SetViewDataFilter : IActionFilter
    {
        private readonly IMenuBarService _MenuBarSer;
        private readonly IMemoryCache _cache;

        public SetViewDataFilter(IMenuBarService MenuBarSer, IMemoryCache cache)
        {
            this._MenuBarSer = MenuBarSer;
            this._cache = cache;
        }

        public void OnActionExecuting(ActionExecutingContext context) 
        {
            // do something before the action executes
            //base.OnActionExecuting(context);
            try
            {
                var userid = Convert.ToInt32(Env.GetUserInfo("Id", context.HttpContext.User.Claims));
                var roleid = Convert.ToInt32(Env.GetUserInfo("RoleId", context.HttpContext.User.Claims));

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                var actionName = descriptor.ActionName.ToLower();
                var controllerName = descriptor.ControllerName.ToLower();
                
                var GetOrPost = context.HttpContext.Request.Method.ToString();
                //var checkAreaName = context.HttpContext.Request.RequestContext.RouteData.DataTokens["area"];
                string AreaName = "";
                //if (checkAreaName != null)
                //{
                //    AreaName = checkAreaName.ToString().ToLower() + "/";
                //}

                var cacheItemKey = "AllMenuBarFilter";

                var globle = _cache.Get(cacheItemKey);

                if (GetOrPost == "POST")
                {
                    ///if menupermission create,edit,delete then update value "true" in IsMenuChange file
                    if (controllerName == "menupermission" && (actionName == "create" || actionName == "edit" || actionName == "delete" || actionName == "multiviewindex"))
                    {
                        globle = MenuBarCache(cacheItemKey, globle, "shortcache");
                    }
                }

                if (globle == null)//if cashe is null
                {
                    globle = MenuBarCache(cacheItemKey, globle, "60mincache");//make cache from db
                }


                var menuaccess = (MenuOfRole[])globle;

                string menuUrl = AreaName + controllerName + "/" + actionName;

                if (IsActionNameEqualToCrudPageName(actionName))
                {
                    menuUrl = AreaName + controllerName;
                }


                var checkUrl = menuaccess.FirstOrDefault(i => (i.MenuURL == AreaName + controllerName + "/" + actionName) || i.MenuURL == menuUrl);
                ///checkUrl: check if menu url Exists in MenuPermission if not exists then will be run
                if (checkUrl != null)
                {
                    var checkControllerActionRoleUserId = menuaccess.FirstOrDefault(i => i.MenuURL == menuUrl && i.RoleId == roleid && i.UserId == userid);
                    ///check menu  && roleid && userid
                    if (checkControllerActionRoleUserId != null)
                    {
                        if (IsActionNameEqualToCrudPageName(actionName))
                        {
                            CheckAccessOfPageAction(context, actionName, checkControllerActionRoleUserId);
                        }
                        else
                        {
                            if (checkControllerActionRoleUserId.IsRead == false || checkControllerActionRoleUserId.IsDelete == false || checkControllerActionRoleUserId.IsCreate == false || checkControllerActionRoleUserId.IsUpdate == false)//if userid !=null && Check Crud
                            {
                                UnAuthoRedirect(context);
                            }
                        }
                    }
                    else
                    {

                        var checkControllerActionRole = menuaccess.FirstOrDefault(i => i.MenuURL == menuUrl && i.RoleId == roleid && i.UserId == null);
                        if (checkControllerActionRole != null)
                        {

                            if (IsActionNameEqualToCrudPageName(actionName))
                            {
                                CheckAccessOfPageAction(context, actionName, checkControllerActionRole);
                            }
                            else
                            {
                                if (checkControllerActionRole.IsRead == false || checkControllerActionRole.IsDelete == false || checkControllerActionRole.IsCreate == false || checkControllerActionRole.IsUpdate == false)//if userid !=null && Check Crud
                                {
                                    UnAuthoRedirect(context);
                                }
                            }
                        }
                        else
                        {
                            if (IsThisAjaxRequest(context) == false)//if userid !=null && Check Crud
                            {
                                UnAuthoRedirect(context);
                            }

                        }


                    }


                }


            }
            catch (Exception)
            { }

        }


        private bool IsActionNameEqualToCrudPageName(string actionName)
        {
            bool ActionIsCrud = false;
            switch (actionName)
            {
                case "create":
                    ActionIsCrud = true;
                    break;
                case "index":
                    ActionIsCrud = true;
                    break;
                case "details":
                    ActionIsCrud = true;
                    break;
                case "edit":
                    ActionIsCrud = true;
                    break;
                case "multiviewindex":
                    ActionIsCrud = true;
                    break;
                case "delete":
                    ActionIsCrud = true;
                    break;
                default:
                    ActionIsCrud = false;
                    break;
            }

            return ActionIsCrud;
        }

        private void CheckAccessOfPageAction(ActionExecutingContext context, string actionName, MenuOfRole checkRoleUrlCrud)
        {
            switch (actionName)
            {
                case "create":
                    if (checkRoleUrlCrud.IsCreate == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;
                case "index":
                    if (checkRoleUrlCrud.IsRead == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;
                case "details":
                    if (checkRoleUrlCrud.IsRead == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;
                case "edit":
                    if (checkRoleUrlCrud.IsUpdate == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;
                case "multiviewindex":
                    if (checkRoleUrlCrud.IsUpdate == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;
                case "delete":
                    if (checkRoleUrlCrud.IsDelete == false)//Check Crud
                    {
                        UnAuthoRedirect(context);
                    }
                    break;

                default:
                    break;
            }
        }

        private dynamic MenuBarCache(string cacheItemKey, dynamic globle, string cachecaption)
        {
           
            if (cachecaption == "shortcache")
            {
                _cache.Remove("AllMenuBar");
                _cache.Remove(cacheItemKey);
            }
            else
            {
                var mp = _MenuBarSer.GetMenuBarlist().Select(m => new MenuOfRole
                {
                    MenuURL = m.Menu_MenuId.MenuURL.ToLower(),
                    RoleId = m.RoleId,
                    UserId = m.UserId,
                    IsCreate = m.IsCreate,
                    IsDelete = m.IsDelete,
                    IsRead = m.IsRead,
                    IsUpdate = m.IsUpdate
                }).Where(i => i.MenuURL != "root").ToArray();
                 
                globle = mp;
                _cache.Set(cacheItemKey, mp, DateTime.Now.AddMinutes(60));
            } 
            return globle;
        }

        private void UnAuthoRedirect(ActionExecutingContext context)
        { 
            var controller = context.Controller as Controller;
            context.Result = controller.RedirectToAction("unauthorized", "auth"); 
        }

        private class MenuOfRole
        {
            public string MenuURL { get; set; }
            public int RoleId { get; set; }
            public Nullable<int> UserId { get; set; }
            public bool IsCreate { get; set; }
            public bool IsRead { get; set; }
            public bool IsUpdate { get; set; }
            public bool IsDelete { get; set; }
        }

        private bool IsThisAjaxRequest(ActionExecutingContext context)
        {
            bool result = false;
            //var currentContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var s = context.HttpContext.Request.Headers["X-Requested-With"];
            if (!string.IsNullOrEmpty(s) && context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                result = true;
            }

            return result;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }

}

