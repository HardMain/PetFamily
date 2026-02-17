using Accounts.Contracts.Requests;
using Core.Abstractions;

namespace Accounts.Application.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequest Request) : ICommand;
}