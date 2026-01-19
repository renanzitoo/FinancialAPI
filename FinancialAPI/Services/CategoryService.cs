using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.DTOs.Responses.Category;
using FinancialAPI.Interfaces;
using FinancialAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FinancialAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _context;

    public CategoryService(ICurrentUserService currentUser, AppDbContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<Guid> CreateCategoryAsync(CategoryRequestDTO dto)
    { 
        var userId = _currentUser.UserId; 
        var category = dto.ToEntity(userId);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        
        return category.Id;
    }
    
    public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
    {
        var userId = _currentUser.UserId;
        
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
    }
    
    public async Task<bool> DeleteCategoryAsync(Guid categoryId)
    {
        var userId = _currentUser.UserId;
        
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
        
        if (category is null)
            return false;
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<CategoryResponseDTO>? GetCategoryByIdAsync(Guid categoryId)
    {
        var userId = _currentUser.UserId;
    
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
        
        return category?.ToResponseDTO()!;
    }

    public async Task<CategoryResponseDTO>? UpdateCategoryAsync(Guid categoryId, CategoryRequestDTO dto)
    {
        var userId = _currentUser.UserId;
        var oldCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
        
        if (oldCategory is null)
            return null;
        
        var updatedCategory = dto.ToEntity(userId);
        updatedCategory.Id = oldCategory.Id;
        
        _context.Entry(oldCategory).CurrentValues.SetValues(updatedCategory);
        await _context.SaveChangesAsync();
        return updatedCategory.ToResponseDTO();
    }
    
}