using ApiCatalog.Data;
using ApiCatalog.Models;
using ApiCatalog.Repositories.Interfaces;

namespace ApiCatalog.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
