namespace FinancialAPI.DTOs.Responses.Auth;

public record AuthResponseDTO
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
};