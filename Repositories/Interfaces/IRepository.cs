using System.Linq.Expressions;

namespace ApiCatalog.Repositories.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> expression);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);

}
