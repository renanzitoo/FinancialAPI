using FinancialAPI.DTOs.Requests.Category;

namespace FinancialAPI.Interfaces;

public interface ICategoryService
{
    Task<Guid> CreateCategoryAsync(CategoryRequestDTO dto, Guid id);
};