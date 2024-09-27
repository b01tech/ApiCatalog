using ApiCatalog.Data;
using ApiCatalog.Models;
using ApiCatalog.Repositories.Interfaces;

namespace ApiCatalog.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository

{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Product> GetByCategory(int id)
    {
        return GetAll().Take(10).Where(c => c.CategoryId == id);
    }
}
