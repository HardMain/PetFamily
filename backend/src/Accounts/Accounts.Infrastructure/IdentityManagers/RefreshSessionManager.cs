using Accounts.Application.Abstractions;
using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Failures;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class RefreshSessionManager(AccountsDbContext accountsContext) 
        : IRefreshSessionManager
    {
        public async Task AddRefreshSession(RefreshSession refreshSession, CancellationToken cancellationToken)
        {
            await accountsContext.AddAsync(refreshSession, cancellationToken);
            await accountsContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
            Guid refreshToken, CancellationToken cancellationToken)
        {
            var refreshSession = await accountsContext.RefreshSessions
                .Include(r => r.User)
                .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);

            if (refreshSession == null)
                return Errors.General.NotFound(refreshToken);

            return refreshSession;
        }

        public async Task Delete(RefreshSession refreshSession, CancellationToken cancellationToken)
        {
            accountsContext.Remove(refreshSession);
            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}