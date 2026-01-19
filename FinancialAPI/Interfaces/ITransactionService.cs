using System.Transactions;
using FinancialAPI.DTOs.Requests.Transactions;
using FinancialAPI.DTOs.Responses.Transaction;

namespace FinancialAPI.Interfaces;

public interface ITransactionService
{
    Task<Guid> CreateTransactionAsync(TransactionRequestDTO dto, Guid userId);
    
}