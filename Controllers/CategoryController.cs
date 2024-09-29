using ApiCatalog.DTOs;
using ApiCatalog.Models;
using ApiCatalog.Pagination;
using ApiCatalog.Repositories.Interfaces;
using ApiCatalog.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ServiceFilter(typeof(LoggingFilter))]
    public ActionResult<IEnumerable<CategoryDTO>> GetAll([FromQuery] PageParameter pageParams)
    {
        try
        {
            var categories = _unitOfWork.categoryRepository.GetList(pageParams);

            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };

            Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoriesDto);
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
    public ActionResult<CategoryDTO> GetById(int id)
    {
        var category = _unitOfWork.categoryRepository.Get(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        var categoryDto = _mapper.Map<CategoryDTO>(category);
        return categoryDto;
    }

    [HttpPost]
    public ActionResult Post([FromBody] CategoryDTO categoryDto)
    {
        if (categoryDto == null)
            return BadRequest("Category is null");

        var category = _mapper.Map<CategoryDto>(categoryDto);
        var newCategory = _unitOfWork.categoryRepository.Create(category);
        _unitOfWork.Commit();
        var newCategoryDto = _mapper.Map<CategoryDTO>(newCategory);
        return CreatedAtRoute("GetCategory", new { id = category.CategoryId }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] CategoryDTO categoryDto)
    {
        if (categoryDto == null)
            return BadRequest("Category is null");

        var category = _mapper.Map<CategoryDto>(categoryDto);

        if (id != category.CategoryId)
        {
            return BadRequest("Id mismatch");
        }
        var newCategory = _unitOfWork.categoryRepository.Update(category);
        _unitOfWork.Commit();

        var newCategoryDto = _mapper.Map<CategoryDTO>(newCategory);
        return Ok(newCategoryDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _unitOfWork.categoryRepository.Get(c => c.CategoryId == id);
        if (category == null)
            return NotFound($"Category {id} not found.");
        var categoryDto = _mapper.Map<CategoryDTO>(category);
        _unitOfWork.categoryRepository.Delete(category);
        _unitOfWork.Commit();
        return Ok(categoryDto);
    }
}
