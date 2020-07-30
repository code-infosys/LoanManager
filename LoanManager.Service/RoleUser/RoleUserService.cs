using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class RoleUserService : Repository<RoleUser>, IRoleUserService
    { 
        public RoleUserService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<RoleUserViewModel> GetGrid(DTParameters param,int user,int role)
        { 
            var conditionData = this.GetAll().AsQueryable();
            if (role == Constants.AdminRole)
                conditionData = this.GetAll().AsQueryable();
            else
            {
                conditionData = this.GetAll().Where(i => i.UserId == user).AsQueryable();
            }

            var tableDataSource = conditionData.Select(m => new RoleUserViewModel { Id = m.Id,RoleId = m.Role_RoleId.RoleName,UserId = m.User_UserId.UserName }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<RoleUserViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<RoleUserViewModel> result = new DTResult<RoleUserViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<RoleUserViewModel> FilterResult(string search, IQueryable<RoleUserViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<RoleUserViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.RoleId.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.UserId.ToString().ToLower().Contains(columnFilters[3].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

