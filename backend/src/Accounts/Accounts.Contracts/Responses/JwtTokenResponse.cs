namespace Accounts.Contracts.Responses
{
    public record JwtTokenResponse(string AccessToken, Guid Jti);
}