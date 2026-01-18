namespace FinancialAPI.Interfaces;

public class ICurrentUserService
{
    private Guid UserId { get; }
    bool IsAuthenticated { get; }
}