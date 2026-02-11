using Accounts.Contracts.Requests;
using Core.Abstractions;

namespace Accounts.Application.Commands.LoginUser
{
    public record LoginCommand(LoginUserRequest Request) : ICommand;
}