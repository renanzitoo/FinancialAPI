using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.DTOs.Requests.Transactions;

public record DateRangeTransactionsRequestDTO
{
    [Required]
    public DateTime? From { get; set; }
    [Required]
    public DateTime? To { get; set; }
}