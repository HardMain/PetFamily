using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Configurations;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.services;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ => new ApplicationDbContext(configuration.GetConnectionString("Database")!));

            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            services.AddHostedService<SoftDeleteCleanupService>();

            services.Configure<SoftDeleteSettings>(configuration.GetSection("SoftDeleteSettings"));

            return services;
        }
    }
}
