using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class AdminAccountManager(AccountsDbContext accountsContext)
    {
        public async Task CreateAdminAccount(AdminAccount adminAccount, CancellationToken cancellationToken)
        {
            await accountsContext.AdminAccounts.AddAsync(adminAccount, cancellationToken);
            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}