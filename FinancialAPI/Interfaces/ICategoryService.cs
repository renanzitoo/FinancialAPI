using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.DTOs.Responses.Category;

namespace FinancialAPI.Interfaces;

public interface ICategoryService
{
    Task<Guid> CreateCategoryAsync(CategoryRequestDTO dto, Guid id);
    Task<bool> DeleteCategoryAsync(Guid categoryId,  Guid userId);
    Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync(Guid userId);
};