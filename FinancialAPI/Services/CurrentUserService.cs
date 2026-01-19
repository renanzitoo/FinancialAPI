using System.Security.Claims;
using FinancialAPI.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinancialAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal? _user;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User;
    }

    public bool IsAuthenticated =>
        _user?.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim =
                _user!.FindFirst(ClaimTypes.NameIdentifier) ??
                _user.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId claim not found.");

            return Guid.Parse(userIdClaim.Value);
        }
    }
}