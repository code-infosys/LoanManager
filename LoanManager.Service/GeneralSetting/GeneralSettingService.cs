using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class GeneralSettingService : Repository<GeneralSetting>, IGeneralSettingService
    { 
        public GeneralSettingService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<GeneralSettingViewModel> GetGrid(DTParameters param)
        { 
            var tableDataSource= this.GetAll().Select(m => new GeneralSettingViewModel { Id = m.Id,SettingKey = m.SettingKey,SettingValue = m.SettingValue,Description = m.Description,SettingGroup = m.SettingGroup }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<GeneralSettingViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<GeneralSettingViewModel> result = new DTResult<GeneralSettingViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<GeneralSettingViewModel> FilterResult(string search, IQueryable<GeneralSettingViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<GeneralSettingViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.SettingKey.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.SettingValue.ToString().ToLower().Contains(columnFilters[3].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[4]))
                    results = results.Where(p => p.Description.ToString().ToLower().Contains(columnFilters[4].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[5]))
                    results = results.Where(p => p.SettingGroup.ToString().ToLower().Contains(columnFilters[5].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

