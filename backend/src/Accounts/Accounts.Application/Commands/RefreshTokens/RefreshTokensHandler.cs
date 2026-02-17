using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Accounts.Application.Abstractions;
using Accounts.Contracts.Responses;
using Accounts.Domain.DataModels;
using Core.Abstractions;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.RefreshTokens
{
    public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
    {
        private readonly IRefreshSessionManager _refreshSessionManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<User> _userManager;

        public RefreshTokensHandler(
            IRefreshSessionManager refreshSessionManager,
            ITokenProvider tokenProvider,
            UserManager<User> userManager)
        {
            _refreshSessionManager = refreshSessionManager;
            _tokenProvider = tokenProvider;
            _userManager = userManager;
        }
        public async Task<Result<LoginResponse, ErrorList>> Handle(
            RefreshTokensCommand command, CancellationToken cancellationToken = default)
        {
            var oldRefreshSessionResult = await _refreshSessionManager
                .GetByRefreshToken(command.RefreshToken, cancellationToken);

            if (oldRefreshSessionResult.IsFailure)
                return oldRefreshSessionResult.Error.ToErrorList();

            if (oldRefreshSessionResult.Value.ExpiresIn < DateTime.UtcNow )
                return Errors.Tokens.ExpiredToken().ToErrorList();

            var userClaimsResult = await _tokenProvider.GetUserClaimsAsync(command.AccessToken, cancellationToken);
            if (userClaimsResult.IsFailure)
                return userClaimsResult.Error.ToErrorList();

            var userIdString = userClaimsResult.Value.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                Errors.Tokens.InvalidToken().ToErrorList();

            if (oldRefreshSessionResult.Value.UserId != userId)
                return Errors.Tokens.InvalidToken().ToErrorList();

            var userJtiString = userClaimsResult.Value.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (!Guid.TryParse(userJtiString, out var userJtiGuid))
                Errors.Tokens.InvalidToken().ToErrorList();

            if (oldRefreshSessionResult.Value.Jti != userJtiGuid)
                return Errors.Tokens.InvalidToken().ToErrorList();

            await _refreshSessionManager.Delete(oldRefreshSessionResult.Value, cancellationToken);

            var roles = await _userManager.GetRolesAsync(oldRefreshSessionResult.Value.User);

            var accessTokenResponse = _tokenProvider
                .GenerateAccessToken(oldRefreshSessionResult.Value.User, roles);
            var refreshToken = await _tokenProvider
                .GenerateRefreshTokenAsync(oldRefreshSessionResult.Value.User, accessTokenResponse.Jti, cancellationToken);

            return new LoginResponse(accessTokenResponse.AccessToken, refreshToken);
        }
    }
}