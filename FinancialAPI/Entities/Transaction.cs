namespace FinancialAPI.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; }
    public string CategoryId { get; set; }
}