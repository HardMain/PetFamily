using Accounts.Contracts.Responses;
using Accounts.Domain.DataModels;
using Core.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.LoginUser
{
    public class LoginHandler : ICommandHandler<LoginResponse, LoginCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            UserManager<User> signInManager,
            ITokenProvider tokenProvider,
            ILogger<LoginHandler> logger)
        {
            _userManager = signInManager;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        public async Task<Result<LoginResponse, ErrorList>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(command.Request.Email);

            if (user is null)
            {
                _logger.LogWarning("Failed to login: user with email {Email} not found", command.Request.Email);

                return Errors.General.NotFound().ToErrorList();
            }

            var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Request.Password);
            if (!passwordConfirmed)
            {
                _logger.LogWarning("Failed to login: invalid credentials for user {UserId}", user.Id);

                return Errors.User.InvalidCredentials().ToErrorList();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var jwtTokenResponse = _tokenProvider.GenerateAccessToken(user, roles);
            var refreshToken = await _tokenProvider.GenerateRefreshTokenAsync(user, jwtTokenResponse.Jti, cancellationToken);

            _logger.LogInformation("User {UserId} successfully logged in", user.Id);
             
            return new LoginResponse(jwtTokenResponse.AccessToken, refreshToken);
        }
    }
}