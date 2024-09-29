using ApiCatalog.Pagination;
using System.Linq.Expressions;

namespace ApiCatalog.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();

    PageList<T> GetList(PageParameter pageParams);
    T? Get(Expression<Func<T, bool>> expression);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);

}
