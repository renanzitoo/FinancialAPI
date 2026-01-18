using System.ComponentModel.DataAnnotations;
using FinancialAPI.Entities;

namespace FinancialAPI.DTOs.Requests.Transactions;

public record TransactionRequestDTO
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public TransactionType Type { get; set; }
    
    public string Description { get; set; }
};