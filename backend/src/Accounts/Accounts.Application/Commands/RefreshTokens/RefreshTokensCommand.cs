using Core.Abstractions;

namespace Accounts.Application.Commands.RefreshTokens
{
    public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;
}