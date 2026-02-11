using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Accounts.Application;
using Accounts.Contracts.Responses;
using Accounts.Domain.DataModels;
using Accounts.Infrastructure.IdentityManagers;
using Accounts.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Failures;

namespace Accounts.Infrastructure.Jwt
{
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshOptions _refreshOptions;
        private readonly RefreshSessionManager _sessionManager;

        public JwtTokenProvider(
            IOptions<JwtOptions> options,
            IOptions<RefreshOptions> refreshOptions,
            RefreshSessionManager sessionManager)
        {
            _jwtOptions = options.Value;
            _refreshOptions = refreshOptions.Value;
            _sessionManager = sessionManager;
        }

        public JwtTokenResponse GenerateAccessToken(User user, IEnumerable<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            var jti = Guid.NewGuid();

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
            }; 

            claims = claims.Concat(roleClaims).ToArray();
             
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
                signingCredentials: creds,
                claims: claims
            );

            var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtTokenResponse(jwtStringToken, jti);
        }

        public async Task<Guid> GenerateRefreshTokenAsync(User user, Guid accessTokenJti, CancellationToken cancellationToken)
        {
            var refreshSession = new RefreshSession
            {
                User = user,
                CreatedAt = DateTime.UtcNow,
                ExpiresIn = DateTime.UtcNow.AddDays(int.Parse(_refreshOptions.ExpiredDaysTime)),
                Jti = accessTokenJti,
                RefreshToken = Guid.NewGuid()
            };

            await _sessionManager.AddRefreshSession(refreshSession, cancellationToken);

            return refreshSession.RefreshToken;
        }

        public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaimsAsync(string jwtToken, CancellationToken cancellationToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

            var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
            if (!validationResult.IsValid)
                return Errors.Tokens.InvalidToken();

            return validationResult.ClaimsIdentity.Claims.ToList();
        }
    }
} 