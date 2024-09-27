using ApiCatalog.Models;
using ApiCatalog.Repositories.Interfaces;
using ApiCatalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _repository;

    public CategoryController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ServiceFilter(typeof(LoggingFilter))]
    public ActionResult<IEnumerable<Category>> GetAll()
    {
        try
        {
            var categories = _repository.GetAll();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error accessing the database.");
        }
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> GetById(int id)
    {
        var category = _repository.Get(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return category;
    }

    [HttpPost]
    public ActionResult Post([FromBody] Category category)
    {
        if (category == null)
            return BadRequest("Category is null");

        _repository.Create(category);
        return CreatedAtRoute("GetCategory", new { id = category.CategoryId }, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Category category)
    {
        if (category == null)
            return BadRequest("Category is null");

        if (id != category.CategoryId)
        {
            return BadRequest("Id mismatch");
        }
        _repository.Update(category);
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.Get(c => c.CategoryId == id);
        if (category == null)
            return NotFound($"Category {id} not found.");
        _repository.Delete(category);
        return Ok(category);
    }
}
