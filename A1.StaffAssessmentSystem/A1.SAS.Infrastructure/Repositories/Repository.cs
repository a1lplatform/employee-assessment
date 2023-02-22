using A1.SAS.Domain.Entities;
using A1.SAS.Domain.Repositories;
using A1.SAS.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace A1.SAS.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly IDbContext _context;

        private readonly DbSet<T> _dbSet;


        public Repository(IDbContext context)
        {
            _context = context;

            _dbSet = context.Set<T>();
        }

        public virtual EntityState Add(T entity)
            => _dbSet.Add(entity).State;

        public T Get<TKey>(TKey id)
            => _dbSet.Find(id);

        public async Task<T> GetAsync<TKey>(TKey id)
            => await _dbSet.FindAsync(id);

        public T Get(params object[] keyValues)
            => _dbSet.Find(keyValues);

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
            => _dbSet.Where(predicate);

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
            => this.FindBy(predicate).Include(include);

        public IQueryable<T> GetAll()
            => _dbSet;

        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;

            return _dbSet.Skip(pageSize).Take(pageCount);
        }

        public IQueryable<T> GetAll(string include)
            => _dbSet.Include(include);

        public IQueryable<T> GetAll(string include, string include2)
            => _dbSet.Include(include).Include(include2);

        public bool Exists(Expression<Func<T, bool>> predicate)
            => _dbSet.Any(predicate);

        public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return _dbSet.Include(navigationPropertyPath);
        }

        public virtual bool AddRange(List<T> entities)
        {
            _dbSet.AddRange(entities);
            return true;
        }
        public virtual bool UpdateRange(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return true;
        }

        T IRepository<T>.Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
        public bool Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            return true;
        }
        public bool DeleteRange(List<T> entities)
        {
            entities.ForEach(e => e.IsDeleted = true);
            _dbSet.UpdateRange(entities);
            return true;
        }
    }
}