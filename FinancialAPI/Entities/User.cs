using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Entities;

public class User
{
    public Guid Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
