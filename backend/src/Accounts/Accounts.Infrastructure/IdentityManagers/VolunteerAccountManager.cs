using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class VolunteerAccountManager(AccountsDbContext accountsContext)
    {
        public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount, CancellationToken cancellationToken)
        {
            await accountsContext.VolunteerAccounts.AddAsync(volunteerAccount, cancellationToken);
            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}