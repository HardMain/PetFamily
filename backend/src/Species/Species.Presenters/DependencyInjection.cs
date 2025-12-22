using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Species.Application;
using Species.Contracts;
using Species.Infrastructure;

namespace Species.Presenters
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSpeciesModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISpeciesContract, SpeciesContract>();

            return services.AddApplication()
                .AddInfrastructure(configuration);
        }
    }
}