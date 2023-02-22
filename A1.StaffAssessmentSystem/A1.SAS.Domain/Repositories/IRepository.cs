using A1.SAS.Domain.Entities;
using System.Linq.Expressions;

namespace A1.SAS.Domain.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Get<TKey>(TKey id);

        Task<T> GetAsync<TKey>(TKey id);

        T Get(params object[] keyValues);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(int page, int pageCount);

        IQueryable<T> GetAll(string include);

        IQueryable<T> GetAll(string include, string include2);

        T Add(T entity);

        bool Exists(Expression<Func<T, bool>> predicate);

        T Update(T entity);

        bool Delete(T entity);

        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

        bool AddRange(List<T> entities);

        bool UpdateRange(List<T> entities);

        bool DeleteRange(List<T> entities);
    }
}
