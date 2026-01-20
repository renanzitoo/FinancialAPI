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
            Date = DateTime.UtcNow,
        };
    }

    public static TransactionListResponseDTO ToListDto(
        this Transaction transaction
    )
    {
        return new TransactionListResponseDTO
        {
            Id = transaction.Id,
            AmountInCents = transaction.AmountInCents,
            TransactionType = transaction.Type.ToString(),
            Date = transaction.Date,
            CategoryName = transaction.Category.Name
        };
    }
}