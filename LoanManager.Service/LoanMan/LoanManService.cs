using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class LoanManService : Repository<LoanMan>, ILoanManService
    { 
        public LoanManService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<LoanManViewModel> GetGrid(DTParameters param)
        { 
            var tableDataSource= this.GetAll().Select(m => new LoanManViewModel { Id = m.Id,DateAdded = m.DateAdded,FullName = m.FullName }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<LoanManViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<LoanManViewModel> result = new DTResult<LoanManViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<LoanManViewModel> FilterResult(string search, IQueryable<LoanManViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<LoanManViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.DateAdded.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.FullName.ToString().ToLower().Contains(columnFilters[3].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

