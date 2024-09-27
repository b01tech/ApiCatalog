using ApiCatalog.Models;
using ApiCatalog.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _repository.GetAll();
        if (products == null || !products.Any())
        {
            return NotFound("Products not found!");
        }
        return Ok(products);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _repository.Get(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found!");
        }
        return Ok(product);
    }

    [HttpGet("Category/{categoryId:int}")]
    public ActionResult<IEnumerable<Product>> GetByCategory(int categoryId)
    {
        var products = _repository.GetByCategory(categoryId);
        if (products == null || !products.Any())
        {
            return NotFound($"No products in category {categoryId}");
        }
        return Ok(products);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Product product)
    {
        if (product == null)
        {
            return BadRequest("Product is null.");
        }

        try
        {
            _repository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating product: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Product product)
    {
        if (product == null)
        {
            return BadRequest("Product is null.");
        }

        if (id != product.ProductId)
        {
            return BadRequest("Product ID mismatch.");
        }

        try
        {
            _repository.Update(product);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating product: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _repository.Get(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found");
        }

        try
        {
            _repository.Delete(product);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting product: {ex.Message}");
        }
    }
}
