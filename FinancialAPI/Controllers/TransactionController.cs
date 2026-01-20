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
    [HttpGet("by-date")]
    public async Task<IActionResult> GetTransactionsByDateInterval([FromQuery]DateRangeTransactionsRequestDTO dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _service.GetTransactionsByDateRangeAsync(dto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("by-category/{categoryId:guid}")]
    public async Task<IActionResult> GetTransactionsByCategory(Guid categoryId)
    {
        var result = await _service.GetTransactionsByCategoryAsync(categoryId);
        return Ok(result);  
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] UpdateTransactionRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _service.UpdateTransactionAsync(id, dto);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var result = await _service.DeleteTransactionAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpGet("summary")]
    public async Task<IActionResult> GetUserSummary()
    {
        var result = await _service.GetUserSummaryAsync();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("summary/{year:int}/{month:int}")]
    public async Task<IActionResult> GetUserSummaryByMonth(int year, int month)
    {
        var result = await _service.GetUserMonthSummaryAsync(year, month);
        return Ok(result);
    }
    
}