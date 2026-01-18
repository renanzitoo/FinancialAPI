using System.Security.Claims;
using FinancialAPI.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinancialAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get;}
    public bool IsAuthenticated { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        
        IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated)
        {
            UserId = Guid.Empty;
            return;
        }
        
        var userIdClaim = 
            user.FindFirst(ClaimTypes.NameIdentifier) ?? 
            user.FindFirst(JwtRegisteredClaimNames.Sub);
        
        UserId = userIdClaim != null
            ? Guid.Parse(userIdClaim.Value)
            : Guid.Empty;
        
    }
}