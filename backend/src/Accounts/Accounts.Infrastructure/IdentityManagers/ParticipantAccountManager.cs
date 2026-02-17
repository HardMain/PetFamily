using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class ParticipantAccountManager(AccountsDbContext accountsContext)
    {
        public async Task CreateParticipantAccount(ParticipantAccount participantAccount, CancellationToken cancellationToken)
        {
            await accountsContext.ParticipantAccounts.AddAsync(participantAccount, cancellationToken);
            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}
