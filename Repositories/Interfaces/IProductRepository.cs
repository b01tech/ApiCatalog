using ApiCatalog.Models;
using ApiCatalog.Pagination;

namespace ApiCatalog.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    PageList<Product> GetByCategory(int id, PageParameter pageParams);
}
