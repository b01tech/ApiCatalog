using ApiCatalog.Models;
using ApiCatalog.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _unitOfWork.productRepository.GetAll();
        if (products == null || !products.Any())
        {
            return NotFound("Products not found!");
        }
        return Ok(products);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _unitOfWork.productRepository.Get(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found!");
        }
        return Ok(product);
    }

    [HttpGet("Category/{categoryId:int}")]
    public ActionResult<IEnumerable<Product>> GetByCategory(int categoryId)
    {
        var products = _unitOfWork.productRepository.GetByCategory(categoryId);
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
            _unitOfWork.productRepository.Create(product);
            _unitOfWork.Commit();
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
            _unitOfWork.productRepository.Update(product);
            _unitOfWork.Commit();
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
        var product = _unitOfWork.productRepository.Get(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found");
        }

        try
        {
            _unitOfWork.productRepository.Delete(product);
            _unitOfWork.Commit();
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting product: {ex.Message}");
        }
    }
}
