namespace FinancialAPI.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    
    public User User { get; set; }
    public Category Category { get; set; }
    
    public string Title { get; set; }
    public long AmountInCents { get; set; }
    public string Description { get; set; }
    
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Income = 1,
    Expense = 2
}