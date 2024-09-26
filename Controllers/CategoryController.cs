using ApiCatalog.Data;
using ApiCatalog.Models;
using ApiCatalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ServiceFilter(typeof(LoggingFilter))]
    public async Task< ActionResult<IEnumerable<Category>>> Get()
    {
        try
        {
            var categories = _context.Categories.AsNoTracking().Take(10).ToListAsync();
            if (categories is null)
            {
                return NotFound();
            }
            return await categories;

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error to access db.");
        }
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> GetById(int id)
    {
        var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return category;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
    {
        var list = _context.Categories.AsNoTracking().Include(c => c.Products).Where(c => c.CategoryId  <= 5).ToList();
        if (list is null)
        {
            return NotFound();
        }
        return list;

    }
        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                return BadRequest();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category is null)
            {
                return NotFound($"Category {id} not found.");
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok(category);
        }
    }
