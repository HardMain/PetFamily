using Accounts.Application;
using Accounts.Contracts;
using Accounts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Presenters
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountsContract, AccountsContract>();

            return services
                .AddApplication()
                .AddInfrastructure(configuration);
        }
    }
}