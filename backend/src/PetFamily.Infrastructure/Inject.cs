using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.SpeciesManagement;
using PetFamily.Application.VolunteersManagement;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure
{
    public static class Inject
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
                ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

            services.AddScoped<WriteDbContext>(_ => new WriteDbContext(connectionString));
            services.AddScoped<IReadDbContext>(_ => new ReadDbContext(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

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

            services.AddSingleton<IFileProvider, MinioProvider>();

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