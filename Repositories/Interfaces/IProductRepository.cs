using ApiCatalog.Models;

namespace ApiCatalog.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetByCategory(int id);
}
