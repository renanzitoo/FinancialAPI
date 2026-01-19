using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.DTOs.Responses.Category;

namespace FinancialAPI.Interfaces;

public interface ICategoryService
{
    Task<Guid> CreateCategoryAsync(CategoryRequestDTO dto);
    Task<bool> DeleteCategoryAsync(Guid categoryId);
    Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync();
    Task<CategoryResponseDTO>? GetCategoryByIdAsync(Guid categoryId);
    Task<CategoryResponseDTO>? UpdateCategoryAsync(Guid categoryId, CategoryRequestDTO dto);
};