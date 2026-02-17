using Accounts.Application.Commands.RegisterUser;
using Accounts.Contracts;
using Accounts.Contracts.Requests;
using Accounts.Infrastructure.IdentityManagers;
using Core.Abstractions;
using SharedKernel.Failures;

namespace Accounts.Presenters
{
    public class AccountsContract(
        ICommandHandler<RegisterUserCommand> handler,
        PermissionManager permissionManager) : IAccountsContract
    {
        public async Task<UnitResult<ErrorList>> RegisterUser(
            RegisterUserRequest request, CancellationToken cancellationToken = default)
        {
            var command = new RegisterUserCommand(request);

            return await handler.Handle(command, cancellationToken);
        }

        public async Task<HashSet<string>> GetUserPermissionCodesByUserId(
            Guid userId, CancellationToken cancellationToken = default)
        {
            return await permissionManager.GetUserPermissionCodesByUserId(userId, cancellationToken);
        }

        public async Task<HashSet<string>> GetUserPermissionCodesByRoles(
            IEnumerable<string> roles, CancellationToken cancellationToken = default)
        {
            return await permissionManager.GetUserPermissionCodesByRoles(roles, cancellationToken);
        }
    }
}