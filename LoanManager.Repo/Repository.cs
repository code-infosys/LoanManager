using Microsoft.EntityFrameworkCore;
using LoanManager.Data;
using LoanManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
namespace LoanManager.Repo
{
    public class Repository<T> : IDisposable, IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext context;
        protected readonly DbSet<T> entities;
        //string errorMessage = string.Empty;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return entities.AsQueryable();
        }
        
        public IQueryable<T> GetAllInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = entities.AsQueryable<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Get(int id)
        {
            return entities.Find(id);
        } 

        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            SaveChange();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Entry(entity).State = EntityState.Modified;
            SaveChange();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            SaveChange();
        }
        
        private void SaveChange()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}

