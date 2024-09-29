using ApiCatalog.Data;
using ApiCatalog.Models;
using ApiCatalog.Pagination;
using ApiCatalog.Repositories.Interfaces;

namespace ApiCatalog.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository

{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public PageList<Product> GetByCategory(int id, PageParameter pageParams)
    {
        var products = GetAll().Where(p => p.CategoryId == id).AsQueryable();
        var prodPaged = PageList<Product>.ToPageList(products, pageParams.PageSize, pageParams.PageNumber);
        return prodPaged;       
    }

}
