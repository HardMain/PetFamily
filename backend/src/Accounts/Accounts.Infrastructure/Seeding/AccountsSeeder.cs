using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Infrastructure.Seeding
{
    public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
    {
        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {

            using var scope = serviceScopeFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<AccountSeederService>();

            await service.SeedAsync(cancellationToken);
        }
    }
}