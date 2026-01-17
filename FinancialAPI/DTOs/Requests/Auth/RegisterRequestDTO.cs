using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.DTOs.Requests;

public record RegisterRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    
    [Required]
    public string Name {get; set;}
    
    [Required]
    public string Password { get; set; }
}