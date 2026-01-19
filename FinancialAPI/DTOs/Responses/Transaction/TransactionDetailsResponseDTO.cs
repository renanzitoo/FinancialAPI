namespace FinancialAPI.DTOs.Responses.Transaction;

public class TransactionDetailsResponseDTO
{
    public Guid Id { get; set; }
    public long AmountInCents { get; set; }
    public string Description { get; set; }
    public string TransactionType { get; set; }
    public DateTime Date { get; set; }
    public string? CategoryName { get; set; }
}