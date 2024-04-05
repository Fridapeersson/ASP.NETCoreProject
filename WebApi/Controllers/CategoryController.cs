using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _categoryRepository.GetAllAsync();

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        try
        {
            var result = await _categoryRepository.GetOneAsync(x => x.Id == id);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
}
