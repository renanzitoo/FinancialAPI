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

        var result = await _service.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        
        var deleted = await _service.DeleteCategoryAsync(id);
        
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllCategoriesAsync();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetCategoryByIdAsync(id)!;
        if(result.Equals(null))
            return NotFound();
        
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, CategoryRequestDTO dto)
    {
        var result = await _service.UpdateCategoryAsync(id, dto)!;
        return Ok(result);
    }
    
}