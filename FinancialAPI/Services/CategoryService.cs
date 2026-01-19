using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.DTOs.Responses.Category;
using FinancialAPI.Interfaces;
using FinancialAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FinancialAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly  JwtService _jwtService;

    public CategoryService(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<Guid> CreateCategoryAsync(CategoryRequestDTO dto, Guid userId)
    {
       var category = dto.ToEntity(userId);
       
       _context.Categories.Add(category);
       await _context.SaveChangesAsync();
       
       return category.Id;
    }
    
    public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync(Guid userId)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
    }
    
    public async Task<bool> DeleteCategoryAsync(Guid categoryId, Guid userId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
        
        if (category is null)
            return false;
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
}