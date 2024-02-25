using System.Linq.Expressions;

namespace Quiz.Core.Interfaces;

public interface IRepository<T> 
    where T : class
{
    Task Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
    IEnumerable<T> Get(Expression<Func<T, bool>> whereCondition);
    Task<T> GetById(params object[] keyValues);

}