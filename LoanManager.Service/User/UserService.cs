using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class UserService : Repository<User>, IUserService
    { 
        public UserService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<UserViewModel> GetGrid(DTParameters param,int user,int role)
        { 
            var conditionData = this.GetAll().AsQueryable();
            if (role == Constants.AdminRole)
                conditionData = this.GetAll().AsQueryable();
            else
            {
                conditionData = this.GetAll().Where(i => i.Id == user).AsQueryable();
            }

            var tableDataSource = conditionData.Select(m => new UserViewModel { Id = m.Id,UserName = m.UserName,Password = m.Password,RoleId = m.RoleId,DateAdded = m.DateAdded,DateModified = m.DateModified,FullName = m.FullName,Email = m.Email,MobileNumber = m.MobileNumber,IsActive = m.IsActive,ChangePasswordCode = m.ChangePasswordCode,IsConfirm = m.IsConfirm,OnTime = m.OnTime,ClockTest = m.ClockTest,About = m.About }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<UserViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<UserViewModel> result = new DTResult<UserViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<UserViewModel> FilterResult(string search, IQueryable<UserViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<UserViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.UserName.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.Password.ToString().ToLower().Contains(columnFilters[3].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[4]))
                    results = results.Where(p => p.RoleId.ToString().ToLower().Contains(columnFilters[4].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[5]))
                    results = results.Where(p => p.DateAdded.ToString().ToLower().Contains(columnFilters[5].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[6]))
                    results = results.Where(p => p.DateModified.ToString().ToLower().Contains(columnFilters[6].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[7]))
                    results = results.Where(p => p.FullName.ToString().ToLower().Contains(columnFilters[7].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[8]))
                    results = results.Where(p => p.Email.ToString().ToLower().Contains(columnFilters[8].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[9]))
                    results = results.Where(p => p.MobileNumber.ToString().ToLower().Contains(columnFilters[9].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[10]))
                    results = results.Where(p => p.IsActive.ToString().ToLower().Contains(columnFilters[10].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[11]))
                    results = results.Where(p => p.ChangePasswordCode.ToString().ToLower().Contains(columnFilters[11].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[12]))
                    results = results.Where(p => p.IsConfirm.ToString().ToLower().Contains(columnFilters[12].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[13]))
                    results = results.Where(p => p.OnTime.ToString().ToLower().Contains(columnFilters[13].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[14]))
                    results = results.Where(p => p.ClockTest.ToString().ToLower().Contains(columnFilters[14].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[15]))
                    results = results.Where(p => p.About.ToString().ToLower().Contains(columnFilters[15].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

