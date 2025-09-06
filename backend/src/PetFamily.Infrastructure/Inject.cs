﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.SpeciesOperations;
using PetFamily.Application.VolunteersOperations;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Infrastructure.BackgroundServices;
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
            services.AddScoped(_ => new WriteDbContext(configuration.GetConnectionString("Database")!));

            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            services.AddHostedService<SoftDeleteCleanupService>();

            services.Configure<SoftDeleteOptions>(configuration.GetSection("SoftDeleteSettings"));

            services.AddHostedService<FilesCleanupBackgroundService>();
            services.AddSingleton<IMessageQueue<IEnumerable<FileStorageDeleteDTO>>, InMemoryMessageQueue<IEnumerable<FileStorageDeleteDTO>>>();

            services.AddMinio(configuration);

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
    }
}