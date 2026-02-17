using System.Security.Claims;
using Accounts.Contracts.Responses;
using Accounts.Domain.DataModels;
using SharedKernel.Failures;

namespace Accounts.Application
{
    public interface ITokenProvider
    {
        JwtTokenResponse GenerateAccessToken(User user, IEnumerable<string> roles); 
        Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaimsAsync(string jwtToken, CancellationToken cancellationToken);
    }
}