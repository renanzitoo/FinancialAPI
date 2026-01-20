using System.Security.Claims;
using FinancialAPI.DTOs.Requests.Transactions;
using FinancialAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAPI.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;
    
    public TransactionController(ITransactionService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]TransactionRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return Unauthorized();
        
        var result = await _service.CreateTransactionAsync(dto);
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserTransactions()
    {
        var result = await _service.GetUserTransactionsAsync();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionDetails(Guid id)
    {
        var result = await _service.GetTransactionDetailsAsync(id);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("/by-date")]
    public async Task<IActionResult> GetTransactionsByDateInterval([FromBody]DateTime fromDate, [FromBody]DateTime toDate)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _service.GetTransactionsByDateRangeAsync(fromDate, toDate);
        return Ok(result);
    }
}