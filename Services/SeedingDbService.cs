using ApiCatalog.Data;
using ApiCatalog.Models;

namespace ApiCatalog.Services;

public class SeedingDbService
{
    private readonly AppDbContext _context;

    public SeedingDbService(AppDbContext context)
    {
        _context = context;
    }

    public void SeedDb()
    {
        if (_context.Categories.Any())
        {
            return;
        }

        CategoryDto c1 = new CategoryDto { Name = "bebidas" , ImageUrl = "bebidas.jpg", CategoryId = 1};

        Product p1 = new Product { Name = "coca-cola", ImageUrl = "coca.jpg", Amount = 10, Price = 5.50m, CategoryId = 1, Description = "coca-cola 2l", RegistrationDate = DateTime.Now };
        Product p2 = new Product { Name = "agua", ImageUrl = "agua.jpg", Amount = 30, Price = 2.50m, CategoryId = 1, Description = "agua 500ml", RegistrationDate = DateTime.Now };
        _context.Categories.Add(c1);
        _context.Products.Add(p1);
        _context.Products.Add(p2);
        _context.SaveChanges();
    }   
}
