using Accounts.Contracts.Requests;
using SharedKernel.Failures;

namespace Accounts.Contracts
{
    public interface IAccountsContract
    {
        Task<UnitResult<ErrorList>> RegisterUser(
            RegisterUserRequest request, CancellationToken cancellationToken = default);

        Task<HashSet<string>> GetUserPermissionCodesByUserId(
            Guid userId, CancellationToken cancellationToken = default);
        Task<HashSet<string>> GetUserPermissionCodesByRoles(
            IEnumerable<string> roles, CancellationToken cancellationToken = default);
    }
} 