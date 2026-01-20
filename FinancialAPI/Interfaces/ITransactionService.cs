using System.Transactions;
using FinancialAPI.DTOs.Requests.Transactions;
using FinancialAPI.DTOs.Responses.Transaction;

namespace FinancialAPI.Interfaces;

public interface ITransactionService
{
    Task<Guid> CreateTransactionAsync(TransactionRequestDTO dto);
    Task<TransactionDetailsResponseDTO> GetTransactionDetailsAsync(Guid transactionId);
    Task<IEnumerable<TransactionListResponseDTO>> GetUserTransactionsAsync();
    Task<bool> DeleteTransactionAsync(Guid transactionId);
    Task<Guid> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequestDTO dto);
    Task<IEnumerable<TransactionListResponseDTO>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<TransactionListResponseDTO>> GetTransactionsByCategoryAsync(Guid categoryId);
}