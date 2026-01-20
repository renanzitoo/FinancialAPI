using System.ComponentModel.DataAnnotations;
using FinancialAPI.Entities;

namespace FinancialAPI.DTOs.Requests.Transactions;

public record TransactionRequestDTO
{
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    
    [Required]
    public long AmountInCents { get; set; }
    
    [Required]
    public TransactionType Type { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime? Date { get; set; }

};