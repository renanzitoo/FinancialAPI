using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests;
using FinancialAPI.Entities;
using FinancialAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;
    
    public AuthController(
        AppDbContext appDbContext,
        JwtService jwtService,
        PasswordService passwordService)
    {
        _appDbContext = appDbContext;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var emailExists = _appDbContext.Users.Any(u=> u.Email == dto.Email);
        if(emailExists)
            return BadRequest("Email already in use.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _passwordService.Hash(dto.Password),
            CreatedAt =  DateTime.UtcNow
        };

        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
        
        return CreatedAtAction(nameof(Register), new
        {
            user.Id,
            user.Name,
            user.Email
        });
        
    }
}