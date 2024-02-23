using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Infrastructure.Data
{
    public class Repository<T> : IRepository<T>
            where T : class
    {
        private QuizDbContext _db;

        public Repository(QuizDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task Add(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _db.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
                _db.Entry(entity).State = EntityState.Unchanged;
            _db.Set<T>().Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return _db.Set<T>().Any(whereCondition);
        }
        
        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.ToList();
        }
        
        public IEnumerable<T> Get(Expression<Func<T, bool>> whereCondition)
        {
            return _db.Set<T>().Where(whereCondition).ToList();
        }

        public async Task<T> GetById(params object[] keyValues)
        {
            return await _db.Set<T>().FindAsync(keyValues);
        }
        
    }
}
