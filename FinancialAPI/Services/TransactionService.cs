using System.Security.Claims;
using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Transactions;
using FinancialAPI.DTOs.Responses.Transaction;
using FinancialAPI.Entities;
using FinancialAPI.Interfaces;
using FinancialAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FinancialAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _context;
    
    public TransactionService(
        AppDbContext context,
        ICurrentUserService currentUser
        )
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Guid> CreateTransactionAsync(TransactionRequestDTO dto)
    {
        var userId = _currentUser.UserId;
        if(userId == Guid.Empty)
            throw new Exception("User not found.");
        
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
        
        if(!categoryExists)
            throw new Exception("Category does not exist.");
        
        dto.Date ??= DateTime.UtcNow;
        
        var transaction = dto.ToEntity(userId);
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return transaction.Id;
    }

    public async Task<TransactionDetailsResponseDTO> GetTransactionDetailsAsync(Guid transactionId)
    {
        var userId = _currentUser.UserId;

        var transaction = await _context.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

        if (transaction == null)
            throw new Exception("Transaction not found.");

        return transaction.ToDetailsDto();
    }

    public async Task<IEnumerable<TransactionListResponseDTO>> GetTransactionsByCategoryAsync(Guid categoryId)
    {
        var userId = _currentUser.UserId;
        
        var transactionsList = await _context.Transactions
            .AsNoTracking()
            .Where(c=> c.UserId == userId && c.CategoryId == categoryId)
            .Include(t => t.Category)
            .ToListAsync();
        return transactionsList.Select(t => t.ToListDto());
    }

    public async Task<IEnumerable<TransactionListResponseDTO>> GetUserTransactionsAsync()
    {
        var userId = _currentUser.UserId;

        var transactionsList = await _context.Transactions
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Include(t => t.Category)
            .ToListAsync();
        return transactionsList.Select(t => t.ToListDto());
    }
    
    public async Task<IEnumerable<TransactionListResponseDTO>> GetTransactionsByDateRangeAsync(DateRangeTransactionsRequestDTO dto)
    {
        var userId = _currentUser.UserId;
        
        if (!dto.From.HasValue || !dto.To.HasValue)
            throw new Exception("Date range is required.");
        
        if(dto.From>dto.To)
            throw new Exception("Invalid date range. 'From' date must be earlier than 'To' date.");

        
        var startDate = dto.From.Value.Date;
        var endDate = dto.To.Value.Date.AddDays(1).AddTicks(-1);
        
        
        var transactionsList = await _context.Transactions
            .Where(c => c.UserId == userId && c.Date >= startDate && c.Date <= endDate)
            .Include(t => t.Category)
            .ToListAsync();
        return transactionsList.Select(t => t.ToListDto());
    }
    
    public async Task<bool> DeleteTransactionAsync(Guid transactionId)
    {
        var userId = _currentUser.UserId;

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

        if (transaction == null)
            return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Guid> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequestDTO dto)
    {
        var userId = _currentUser.UserId;

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

        if (transaction == null)
            throw new Exception("Transaction not found.");

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

        if (!categoryExists)
            throw new Exception("Category does not exist.");

        transaction.Title = dto.Title;
        transaction.Description = dto.Description;
        transaction.AmountInCents = dto.AmountInCents;
        transaction.Type = dto.Type;
        transaction.CategoryId = dto.CategoryId;
        
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        
        return transaction.Id;
    }
    
    public async Task<TransactionsSumaryDTO> GetUserSummaryAsync()
    {
        var userId = _currentUser.UserId;

        var incomeTotal = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Income)
            .SumAsync(t => (long?)t.AmountInCents) ?? 0;

        var expenseTotal = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .SumAsync(t => (long?)t.AmountInCents) ?? 0;

        var balance = incomeTotal - expenseTotal;

        return new TransactionsSumaryDTO
        {
            TotalIncomeInCents = incomeTotal,
            TotalExpenseInCents = expenseTotal,
            NetBalanceInCents = balance
        };
    }
    
    public async Task<TransactionsSumaryDTO> GetUserMonthSummaryAsync(int year, int month)
    {
        var userId = _currentUser.UserId;
        
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1);

        var incomeTotal = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Income &&
                        t.Date >= startDate &&
                        t.Date < endDate)
            .SumAsync(t => (long?)t.AmountInCents) ?? 0;

        var expenseTotal = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense&&
                        t.Date >= startDate &&
                        t.Date < endDate)
            .SumAsync(t => (long?)t.AmountInCents) ?? 0;

        var balance = incomeTotal - expenseTotal;

        return new TransactionsSumaryDTO
        {
            TotalIncomeInCents = incomeTotal,
            TotalExpenseInCents = expenseTotal,
            NetBalanceInCents = balance
        };
    }


}