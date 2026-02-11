using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Volunteers.Infrastructure.BackgroundServices;
using Volunteers.Infrastructure.Options;
using Volunteers.Infrastructure;
using Volunteers.Infrastructure.Repositories;
using Volunteers.Infrastructure.Providers;
using Volunteers.Application.Abstractions;
using Volunteers.Infrastructure.DbContexts;
using Core.Providers;
using SharedKernel.ValueObjects;
using Volunteers.Infrastructure.MessageQueues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core.Abstractions;

namespace Volunteers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddRepositories()
                .AddMinio(configuration)
                .AddMessageQueues()
                .AddOptions(configuration)
                .AddHostedServices();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database")
                ?? throw new InvalidOperationException("Connection string 'Database' not found.");
            var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

            services.AddDbContext<VolunteersReadDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .UseLoggerFactory(loggerFactory)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<VolunteersWriteDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .UseLoggerFactory(loggerFactory));

            services.AddScoped<IVolunteersReadDbContext>(sp =>
                sp.GetRequiredService<VolunteersReadDbContext>());
            
            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            return services;
        }

        private static IServiceCollection AddMinio(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMinio(options =>
            {
                var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                                   ?? throw new ApplicationException("Missing minio configuration");

                options.WithEndpoint(minioOptions.Endpoint);
                options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
                options.WithSSL(minioOptions.WithSSL);
            });

            services.AddSingleton<IFilesProvider, MinioProvider>();

            return services;
        }

        private static IServiceCollection AddMessageQueues(
            this IServiceCollection services)
        {
            services.AddSingleton<IMessageQueue<IEnumerable<FileStorageDeleteDto>>,
                InMemoryMessageQueue<IEnumerable<FileStorageDeleteDto>>>();

            return services;

        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SoftDeleteOptions>(configuration.GetSection("SoftDeleteSettings"));

            return services;
        }

        private static IServiceCollection AddHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<SoftDeleteCleanupService>();
            services.AddHostedService<FilesCleanupBackgroundService>();

            return services;

        }
    }
}