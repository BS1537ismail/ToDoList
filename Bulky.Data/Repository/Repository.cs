using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        private DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
            //dbcontext.Categories == dbSet
            dbContext.Products.Include(x => x.Category).Include(x => x.CategoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> predicate, string? inclideProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(predicate);

            if (!string.IsNullOrEmpty(inclideProperties))
            {
                foreach (var includeProp in inclideProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? inclideProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(inclideProperties))
            {
                foreach (var includeProp in inclideProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}