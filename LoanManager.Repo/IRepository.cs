using LoanManager.Data;
using LoanManager.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace LoanManager.Repo
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQueryable();
        T Get(int id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllInclude(params Expression<Func<T, object>>[] includeProperties);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

