using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Species.Infrastructure.DbContexts;
using Species.Application.Abstractions;
using Species.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Species.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database")
                ?? throw new InvalidOperationException("Connection string 'Database' not found.");
            var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

            services.AddDbContext<SpeciesReadDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .UseLoggerFactory(loggerFactory)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<SpeciesWriteDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .UseLoggerFactory(loggerFactory));

            services.AddScoped<ISpeciesReadDbContext>(sp =>
                sp.GetRequiredService<SpeciesReadDbContext>());

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            return services;
        }
    }
}