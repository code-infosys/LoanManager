using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class MenuPermissionService : Repository<MenuPermission>, IMenuPermissionService
    { 
        public MenuPermissionService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<MenuPermissionViewModel> GetGrid(DTParameters param,int user,int role)
        { 
            var conditionData = this.GetAll().AsQueryable();
            if (role == Constants.AdminRole)
                conditionData = this.GetAll().AsQueryable();
            else
            {
                conditionData = this.GetAll().Where(i => i.UserId == user).AsQueryable();
            }

            var tableDataSource = conditionData.Select(m => new MenuPermissionViewModel { Id = m.Id,MenuId = m.Menu_MenuId.MenuText,RoleId = m.Role_RoleId.RoleName,UserId = m.User_UserId.UserName,SortOrder = m.SortOrder,IsCreate = m.IsCreate,IsRead = m.IsRead,IsUpdate = m.IsUpdate,IsDelete = m.IsDelete,IsActive = m.IsActive }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<MenuPermissionViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<MenuPermissionViewModel> result = new DTResult<MenuPermissionViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<MenuPermissionViewModel> FilterResult(string search, IQueryable<MenuPermissionViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<MenuPermissionViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.MenuId.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.RoleId.ToString().ToLower().Contains(columnFilters[3].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[4]))
                    results = results.Where(p => p.UserId.ToString().ToLower().Contains(columnFilters[4].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[5]))
                    results = results.Where(p => p.SortOrder.ToString().ToLower().Contains(columnFilters[5].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[6]))
                    results = results.Where(p => p.IsCreate.ToString().ToLower().Contains(columnFilters[6].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[7]))
                    results = results.Where(p => p.IsRead.ToString().ToLower().Contains(columnFilters[7].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[8]))
                    results = results.Where(p => p.IsUpdate.ToString().ToLower().Contains(columnFilters[8].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[9]))
                    results = results.Where(p => p.IsDelete.ToString().ToLower().Contains(columnFilters[9].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[10]))
                    results = results.Where(p => p.IsActive.ToString().ToLower().Contains(columnFilters[10].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

