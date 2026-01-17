using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.DTOs.Requests;

public record LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
};