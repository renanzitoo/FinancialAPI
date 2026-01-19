using FinancialAPI.DTOs.Requests.Category;
using FinancialAPI.DTOs.Responses.Category;
using FinancialAPI.Entities;

namespace FinancialAPI.Mappings;

public static class CategoryMapping
{
    public static Category ToEntity(this CategoryRequestDTO dto, Guid userId)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            UserId = userId
        };
    }
    
    public static CategoryResponseDTO ToResponseDTO(this Category entity)
    {
        return new CategoryResponseDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}