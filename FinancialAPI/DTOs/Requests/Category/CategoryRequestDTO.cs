using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.DTOs.Requests.Category;

public record CategoryRequestDTO
{
    [Required]
    public string Name { get; set; }
};