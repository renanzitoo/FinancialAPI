using FinancialAPI.DTOs.Requests.Transactions;
using FinancialAPI.DTOs.Responses.Transaction;
using FinancialAPI.Entities;

namespace FinancialAPI.Mappings;

public static class TransactionMapping
{
    public static Transaction ToEntity(
        this TransactionRequestDTO dto,
        Guid userId
    )
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            AmountInCents = dto.AmountInCents,
            Type = dto.Type,
            Date = dto.Date ?? DateTime.Now,
        };
    }
    
    public static Transaction ToEntity(
        this UpdateTransactionRequestDTO dto,
        Transaction existingTransaction
    )
    {
        existingTransaction.Title = dto.Title;
        existingTransaction.CategoryId = dto.CategoryId;
        existingTransaction.AmountInCents = dto.AmountInCents;
        existingTransaction.Type = dto.Type;
        existingTransaction.Description = dto.Description;
        existingTransaction.Date = dto.Date;

        return existingTransaction;
    }

    public static TransactionListResponseDTO ToListDto(
        this Transaction transaction
    )
    {
        return new TransactionListResponseDTO
        {
            Id = transaction.Id,
            Name = transaction.Title,
            AmountInCents = transaction.AmountInCents,
            TransactionType = transaction.Type.ToString(),
            Date = transaction.Date,
            CategoryName = transaction.Category.Name
        };
    }
    
    public static TransactionDetailsResponseDTO ToDetailsDto(
        this Transaction transaction
    )
    {
        return new TransactionDetailsResponseDTO
        {
            Id = transaction.Id,
            AmountInCents = transaction.AmountInCents,
            Description = transaction.Description,
            TransactionType = transaction.Type.ToString(),
            Date = transaction.Date,
            CategoryName = transaction.Category.Name
        };
    }
}