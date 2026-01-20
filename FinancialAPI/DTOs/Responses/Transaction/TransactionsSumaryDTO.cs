namespace FinancialAPI.DTOs.Requests.Transactions;

public record TransactionsSumaryDTO
{
    public long TotalIncomeInCents { get; set; }
    public long TotalExpenseInCents { get; set; }
    public long NetBalanceInCents { get; set; }
};