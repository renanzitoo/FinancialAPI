using System.Security.Claims;
using FinancialAPI.Context;
using FinancialAPI.DTOs.Requests;
using FinancialAPI.Entities;
using FinancialAPI.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDTO dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = _appDbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
        if(user is null)
            return Unauthorized("Invalid email or password.");

        var passwordValid = _passwordService.Verify(
            user.PasswordHash,
            dto.Password);
        
        if(!passwordValid)
            return Unauthorized("Invalid email or password.");
        
        var authResponse = _jwtService.GenerateToken(user);
        
        return Ok(authResponse);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue(ClaimTypes.Name);

        return Ok(new
        {
            Id = id,
            Name = name,
            Email = email
        });
    }
}