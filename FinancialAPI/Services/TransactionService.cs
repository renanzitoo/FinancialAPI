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
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    
    public TransactionService(
        AppDbContext context,
        JwtService jwtService
    )
    {
        _context = context;
        _jwtService = jwtService;
    }
    
    public async Task<Guid> CreateTransactionAsync(TransactionRequestDTO dto, Guid userId)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
        
        if(!categoryExists)
            throw new Exception("Category does not exist.");
        
        var transaction = dto.ToEntity(userId);
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return transaction.Id;
    }
}