using System.Security.Claims;
using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests.Transactions;
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
        
        var transaction = dto.ToEntity(userId);
        transaction.Category = _context.Categories.Find(dto.CategoryId);
        transaction.User = _context.Users.Find(userId);
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return transaction.Id;
    }
}