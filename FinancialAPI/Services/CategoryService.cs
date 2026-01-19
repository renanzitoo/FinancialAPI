using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.Interfaces;
using FinancialAPI.Mappings;

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
    
}