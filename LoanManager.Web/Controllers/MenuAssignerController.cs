using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoanManager.Models;
using LoanManager.Service;

namespace LoanManager.Web.Controllers
{
    public class MenuAssignerController : BaseController
    {
        private readonly IMenuPermissionService _menuPermissionService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        public MenuAssignerController(IMenuPermissionService menuPermissionService, IRoleService roleService, IUserService userService, IMenuService menuSerice)
        {
            this._menuPermissionService = menuPermissionService;
            this._roleService = roleService;
            this._userService = userService;
            this._menuService = menuSerice;
        }
        public IActionResult Index(int roleId = 1)
        {
            var role = _roleService.GetAll().ToArray();
            ViewBag.Roles = new SelectList(role, "Id", "RoleName", roleId);
            ViewBag.Users = new SelectList(_userService.GetAll().ToArray(), "Id", "UserName");
            ViewBag.RoleName = role.FirstOrDefault(i => i.Id == roleId).RoleName;
            ViewBag.RoleId = roleId;
            var dtsource = _menuService.GetAllInclude(i => i.MenuPermission_MenuIds).ToArray();

            return View(dtsource);
        }

        [HttpPost]
        public IActionResult AddMenuPermission(int menuId, int roleId, Nullable<int> sortOrder, Nullable<int> user)
        {
            AlertBack alert = new AlertBack();
            try
            {
                MenuPermission menu = new MenuPermission
                {
                    IsCreate = true,
                    IsDelete = true,
                    IsRead = true,
                    IsUpdate = true,
                    IsActive=true,
                    MenuId = menuId,
                    RoleId = roleId,
                    UserId = user,
                    SortOrder = sortOrder
                };
                _menuPermissionService.Insert(menu);
                alert.Status = "success";
                alert.Message = menu.Id.ToString();

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
            AlertBack alert = new AlertBack();

            MenuPermission ObjMenuPermission = _menuPermissionService.Get(id);
            _menuPermissionService.Delete(ObjMenuPermission);
            alert.Status = "success";
            alert.Message = "Register Successfully";

            return Json(alert);
        }

        [HttpPost]
        public IActionResult Update(int id, string fieldName, string value)
        {
            AlertBack alert = new AlertBack();
            MenuPermission model = _menuPermissionService.Get(id);
            if (fieldName == "SortOrder")
                model.SortOrder = int.Parse(value);
            else if (fieldName == "IsCreate")
                model.IsCreate = bool.Parse(value);
            else if (fieldName == "IsRead")
                model.IsRead = bool.Parse(value);
            else if (fieldName == "IsUpdate")
                model.IsUpdate = bool.Parse(value);
            else if (fieldName == "IsDelete")
                model.IsDelete = bool.Parse(value);
            else if (fieldName == "IsActive")
                model.IsActive = bool.Parse(value);

            _menuPermissionService.Update(model);
            alert.Status = "success";
            alert.Message = "Updated Successfully";

            return Json(alert);
        }
 

    }
}
