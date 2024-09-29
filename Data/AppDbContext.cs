using ApiCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<CategoryDto> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

}
