using Accounts.Domain.DataModels;
using SharedKernel.Failures;

namespace Accounts.Application.Abstractions
{
    public interface IRefreshSessionManager
    {
        Task AddRefreshSession(RefreshSession refreshSession, CancellationToken cancellationToken);
        Task Delete(RefreshSession refreshSession, CancellationToken cancellationToken);
        Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken);
    }
}