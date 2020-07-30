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
using Microsoft.Extensions.Localization;

namespace LoanManager.Web
{  
    public class MenuBarViewComponent : ViewComponent
    {
        private readonly IMenuBarService _MenuBarSer; 
        private readonly IMemoryCache _cache;
        private const int userRole = 2;
        public IStringLocalizer localizer { get; set; }
        public MenuBarViewComponent(IMenuBarService MenuBarSer , IMemoryCache cache, IStringLocalizer<string> _localizer)
        {
            this._MenuBarSer = MenuBarSer; 
            this._cache = cache;
            localizer = _localizer;
        }

        //public IViewComponentResult Invoke()
        //{
        //    //var articles = _articleService.GetNewArticles(numberOfItems);
        //    //return View(articles);
        //    return View();
        //}


        public HtmlString Invoke(IEnumerable<Claim> Claim)
        {
            StringBuilder sb = new StringBuilder();
            int? ParentId = null;

             //get role id and role regarding to role bind this
            var UserId = Convert.ToInt32(Env.GetUserInfo("Id", Claim));
            var RoleId = Convert.ToInt32(Env.GetUserInfo("RoleId", Claim));

            //var cacheItemKey = "jApMenuBar" + userId + "Us" + RoleId;
            var cacheItemKey = "AllMenuBar";
             
            var globle = (List<MenuPermission>)_cache.Get(cacheItemKey);
           
            if (globle == null)
            {
                globle = _MenuBarSer.GetMenuBarlist().ToList();
                //listMenuPer = (List<MenuPermission>)globle;
                _cache.Set(cacheItemKey, globle, DateTime.Now.AddMinutes(60));
            }

            string dashboard = "/Home";
            if (RoleId == userRole)
                dashboard = "/Home/UserIndex";

             
            sb.Append("<ul class=\"sidebar-menu\">");
            sb.Append("<li class=\"active\"> <a href=\""+ dashboard + "\"> <i class=\"fa fa-dashboard\"></i> <span>"+ localizer["Dashboard"] + " </span> </a> </li>");

            sb.Append(GetMenuBar(ParentId, globle.Where(i => (i.RoleId == RoleId && i.UserId == null) || i.UserId == UserId).ToArray()));
            sb.Append("</ul>"); 
            return new HtmlString(sb.ToString());
        }




        private HtmlString GetMenuBar(int? ParentId, MenuPermission[] q)
        {
            StringBuilder sb = new StringBuilder();
            if (q != null)
            {
                foreach (var item in q.Where(i => i.Menu_MenuId.ParentId == ParentId).OrderBy(i => i.SortOrder))
                {
                    var js = q;

                    if (js.Count(j => j.Menu_MenuId.ParentId == item.Menu_MenuId.Id) > 0)
                    {
                        //ViewBag.OpenMenu is used for ,supose if menu text name you want to Different then controller name
                        //then in cshtml page top Add ViewBag.OpenMenu="Give that name which you Given in Menu Text";
                        string active = "";
                        string style = "";
                        try
                        {
                            string controllerName = base.ViewContext.RouteData.Values["Controller"].ToString().ToSpaceBeforeUpperChar();
                            controllerName = ViewBag.OpenMenu == null ? controllerName : ViewBag.OpenMenu;
                            var menucheck = q.FirstOrDefault(i => i.Menu_MenuId.MenuText == controllerName && i.Menu_MenuId.ParentId != null);
                            if (menucheck != null)
                            {
                                var openMenu = menucheck.Menu_MenuId.Menu2.MenuText;
                                if (openMenu == item.Menu_MenuId.MenuText)
                                {
                                    active = " menu-open";
                                    style = "style=\"display: block;\"";
                                }
                            }
                        }
                        catch (Exception) { }

                        if (item.Menu_MenuId.ParentId == null)
                        {
                            sb.Append("<li class=\"treeview " + active + "\"> <a href=\"#\"> " + item.Menu_MenuId.MenuIcon + " <span>" + localizer[item.Menu_MenuId.MenuText.Trim()] + "</span> <i class=\"fa fa-angle-left pull-right\"></i>  </a><ul class=\"treeview-menu \" " + style + ">");
                        }
                        else
                        {
                            sb.Append("<li class=\"treeview\"> <a href=\"#\"> " + item.Menu_MenuId.MenuIcon + " <span>" + localizer[item.Menu_MenuId.MenuText.Trim()] + "</span> <i class=\"fa fa-angle-left pull-right\"></i>  </a><ul class=\"treeview-menu\">");
                        }
                        sb.Append(GetMenuBar(item.Menu_MenuId.Id, q));
                    }
                    else
                    {
                        if (item.Menu_MenuId.ParentId == null)
                        {
                            sb.Append("<li class=\"\"> <a href=\"/" + item.Menu_MenuId.MenuURL + "\"> " + item.Menu_MenuId.MenuIcon + " <span>" + localizer[item.Menu_MenuId.MenuText.Trim()] + "</span></a></li>");
                        }
                        else
                        {
                            sb.Append("<li class=\"\"> <a href=\"/" + item.Menu_MenuId.MenuURL + "\"> " + item.Menu_MenuId.MenuIcon + " <span>" + localizer[item.Menu_MenuId.MenuText.Trim()] + "</span></a></li>");
                        } 
                    }

                }
                sb.Append("</ul>");
            }
             
            return new HtmlString(sb.ToString());
        }

    }
}


