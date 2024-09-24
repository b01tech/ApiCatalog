using ApiCatalog.Data;
using ApiCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.ToList();
        if (products is null)
        {
            return NotFound("Products not found!");
        }
        return products;
    }

    [HttpGet("{id:int}", Name = "GetProduct")]

    public ActionResult<Product> GetById(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found!");
        }
        return product;
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
        {
            return BadRequest();
        }
        _context.Products.Add(product);
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);

    }
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }
        _context.Entry(product).State = EntityState.Modified;
        _context.Products.Update(product);
        _context.SaveChanges();
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => (p.ProductId == id));
        if (product is null)
        {
            return NotFound($"Product {id} not found");
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        return Ok(product);
    }
}
