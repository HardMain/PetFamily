using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Accounts.Application.Abstractions;
using Accounts.Contracts.Responses;
using Accounts.Domain.DataModels;
using Core.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.RefreshTokens
{
    public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
    {
        private readonly IRefreshSessionManager _refreshSessionManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RefreshTokensHandler> _logger;

        public RefreshTokensHandler(
            IRefreshSessionManager refreshSessionManager,
            ITokenProvider tokenProvider,
            UserManager<User> userManager,
            ILogger<RefreshTokensHandler> logger)
        {
            _refreshSessionManager = refreshSessionManager;
            _tokenProvider = tokenProvider;
            _userManager = userManager;
            _logger = logger;
        }

        //добавить логи в ошибках
        public async Task<Result<LoginResponse, ErrorList>> Handle(
            RefreshTokensCommand command, CancellationToken cancellationToken = default)
        {
            var oldRefreshSessionResult = await _refreshSessionManager
                .GetByRefreshToken(command.RefreshToken, cancellationToken);

            if (oldRefreshSessionResult.IsFailure)
            {
                _logger.LogWarning("Failed to get refresh session {RefreshToken}: {Errors}", 
                    command.RefreshToken, oldRefreshSessionResult.Error);

                return oldRefreshSessionResult.Error.ToErrorList();
            }

            if (oldRefreshSessionResult.Value.ExpiresIn < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token {RefreshToken} is expired", command.RefreshToken);

                return Errors.Tokens.ExpiredToken().ToErrorList();
            }

            var userClaimsResult = await _tokenProvider.GetUserClaimsAsync(command.AccessToken, cancellationToken);
            if (userClaimsResult.IsFailure)
            {
                _logger.LogWarning("Failed to get claims from access token: {Errors}", userClaimsResult.Error);

                return userClaimsResult.Error.ToErrorList();
            }

            var userIdString = userClaimsResult.Value.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("Invalid user_id in access token claims");

                return Errors.Tokens.InvalidToken().ToErrorList();
            }

            if (oldRefreshSessionResult.Value.UserId != userId)
            {
                _logger.LogWarning("User id mismatch: token belongs to {TokenUserId}, request from {UserId}",
                    oldRefreshSessionResult.Value.UserId, userId);
                
                return Errors.Tokens.InvalidToken().ToErrorList();
            }

            var userJtiString = userClaimsResult.Value.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (!Guid.TryParse(userJtiString, out var userJtiGuid))
            {
                _logger.LogWarning("Invalid jti in access token claims");

                return Errors.Tokens.InvalidToken().ToErrorList();
            }

            if (oldRefreshSessionResult.Value.Jti != userJtiGuid)
            {
                _logger.LogWarning("Jti mismatch for user {UserId}", userId);

                return Errors.Tokens.InvalidToken().ToErrorList();
            }

            await _refreshSessionManager.Delete(oldRefreshSessionResult.Value, cancellationToken);

            var roles = await _userManager.GetRolesAsync(oldRefreshSessionResult.Value.User);

            var accessTokenResponse = _tokenProvider
                .GenerateAccessToken(oldRefreshSessionResult.Value.User, roles);
            var refreshToken = await _tokenProvider
                .GenerateRefreshTokenAsync(oldRefreshSessionResult.Value.User, accessTokenResponse.Jti, cancellationToken);

            _logger.LogInformation("Tokens successfully refreshed for user {UserId}.", userId);

            return new LoginResponse(accessTokenResponse.AccessToken, refreshToken);
        }
    }
}