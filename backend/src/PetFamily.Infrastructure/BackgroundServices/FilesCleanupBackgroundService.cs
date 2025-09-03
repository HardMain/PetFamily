﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public class FilesCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FilesCleanupBackgroundService> _logger;
        private readonly IMessageQueue<IEnumerable<FileStorageDeleteDTO>> _messageQueue;
        private readonly IFileProvider _fileProvider;

        public FilesCleanupBackgroundService(
            ILogger<FilesCleanupBackgroundService> logger, 
            IMessageQueue<IEnumerable<FileStorageDeleteDTO>> messageQueue,
            IFileProvider fileProvider,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _messageQueue = messageQueue;
            _fileProvider = fileProvider;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FilesCleanupBackgroundService is starting");

            await using var scope = _scopeFactory.CreateAsyncScope();

            var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

            while (stoppingToken.IsCancellationRequested)
            {
                var fileSToDelete = await _messageQueue.ReadAsync(stoppingToken);

                var deleteResult = await _fileProvider.DeleteFiles(fileSToDelete, stoppingToken);
                if (deleteResult.IsFailure)
                    _logger.LogDebug("FilesCleanupBackgroundService: Failed to clean up MinIO files - {Errors}",
                        deleteResult.Error);
                else
                    _logger.LogWarning("FilesCleanupBackgroundService: Files deleted - {DeletedFiles}",
                        deleteResult.Value);
            }
        }
    }
}
