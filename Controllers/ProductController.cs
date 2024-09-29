using ApiCatalog.DTOs;
using ApiCatalog.Models;
using ApiCatalog.Pagination;
using ApiCatalog.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery]PageParameter pageParams)
    {
        var products = _unitOfWork.productRepository.GetList(pageParams);

        var metadata = new
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious,
        };

        Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(metadata));

        if (!products.Any()) 
            return NotFound("No products found.");
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDto);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public ActionResult<ProductDTO> GetById(int id)
    {
        var product = _unitOfWork.productRepository.Get(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound($"Product id {id} not found!");
        }
        var productDto = _mapper.Map<Product>(product);
        return Ok(productDto);
    }

    [HttpGet("Category/{categoryId:int}")]
    public ActionResult<IEnumerable<ProductDTO>> GetByCategory(int categoryId)
    {
        var products = _unitOfWork.productRepository.GetByCategory(categoryId);
        if (products == null || !products.Any())
        {
            return NotFound($"No products in category {categoryId}");
        }
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDto);
    }

    [HttpPost]
    public ActionResult Post([FromBody] ProductDTO productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Product is null.");
        }

        try
        {
            var prod = _mapper.Map<Product>(productDto);
            _unitOfWork.productRepository.Create(prod);
            _unitOfWork.Commit();
            return CreatedAtRoute("GetProduct", new { id = prod.ProductId }, productDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating product: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] ProductDTO productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Product is null.");
        }
        var prod = _mapper.Map<Product>(productDto);

        if (id != prod.ProductId)
        {
            return BadRequest("Product ID mismatch.");
        }

        try
        {
            _unitOfWork.productRepository.Update(prod);
            _unitOfWork.Commit();
            return Ok(productDto);
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
            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting product: {ex.Message}");
        }
    }
}
