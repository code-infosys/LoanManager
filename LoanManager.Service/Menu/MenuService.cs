using LoanManager.Models;
using LoanManager.Repo;  
using LoanManager.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Dynamic.Core;
namespace LoanManager.Service
{
    public class MenuService : Repository<Menu>, IMenuService
    { 
        public MenuService(ApplicationContext dbContext) : base(dbContext) {} 
        public DTResult<MenuViewModel> GetGrid(DTParameters param)
        { 
            var tableDataSource= this.GetAll().Select(m => new MenuViewModel { Id = m.Id,MenuText = m.MenuText,MenuURL = m.MenuURL,ParentId = m.Menu2.MenuText,SortOrder = m.SortOrder,MenuIcon = m.MenuIcon }).AsQueryable();

            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            var filterdData= FilterResult(param.Search.Value, tableDataSource, columnSearch, param.SearchFromLength);
            List<MenuViewModel> data = filterdData.OrderBy(param.SortOrder).Skip(param.Start).Take(param.Length).ToList();
            int count = filterdData.Count();

            DTResult<MenuViewModel> result = new DTResult<MenuViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return result;
             
        }

        private IQueryable<MenuViewModel> FilterResult(string search, IQueryable<MenuViewModel> dtResult, List<string> columnFilters, int searchTake = 500)
        {
            IQueryable<MenuViewModel> results = dtResult;
            if (searchTake == 0)
                results = results.OrderByDescending(i => i.Id).AsQueryable();
            else
                results = results.OrderByDescending(i => i.Id).Take(searchTake).AsQueryable();

            if (!columnFilters.All(x => string.IsNullOrWhiteSpace(x)))
            {
                if (!string.IsNullOrEmpty(columnFilters[1]))
                    results = results.Where(p => p.Id.ToString().ToLower().Contains(columnFilters[1].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[2]))
                    results = results.Where(p => p.MenuText.ToString().ToLower().Contains(columnFilters[2].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[3]))
                    results = results.Where(p => p.MenuURL.ToString().ToLower().Contains(columnFilters[3].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[4]))
                    results = results.Where(p => p.ParentId.ToString().ToLower().Contains(columnFilters[4].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[5]))
                    results = results.Where(p => p.SortOrder.ToString().ToLower().Contains(columnFilters[5].ToLower()));
                if (!string.IsNullOrEmpty(columnFilters[6]))
                    results = results.Where(p => p.MenuIcon.ToString().ToLower().Contains(columnFilters[6].ToLower()));

            }
            return results.AsQueryable();
        }
    }
}

