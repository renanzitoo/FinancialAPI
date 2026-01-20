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
}