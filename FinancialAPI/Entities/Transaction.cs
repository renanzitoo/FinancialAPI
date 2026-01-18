namespace FinancialAPI.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    
    public int Amount { get; set; }
    public string Description { get; set; }
    
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Income = 1,
    Expense = 2
}