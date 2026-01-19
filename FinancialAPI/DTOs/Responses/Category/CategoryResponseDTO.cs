namespace FinancialAPI.DTOs.Responses.Category;

public record CategoryResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
};