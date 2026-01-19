using System.Security.Claims;
using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.Interfaces;
using FinancialAPI.Mappings;
using FinancialAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAPI.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CategoryRequestDTO dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId is null)
            return Unauthorized();

        var result = await _service.CreateCategoryAsync(dto, new Guid(userId));
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var deleted = await _service.DeleteCategoryAsync(id,new Guid(userId));
        
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var result = await _service.GetAllCategoriesAsync(new Guid(userId));
        return Ok(result);
    }
    
    
    
}