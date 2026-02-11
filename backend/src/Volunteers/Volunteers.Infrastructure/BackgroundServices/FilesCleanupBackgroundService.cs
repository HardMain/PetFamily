using Core.Abstractions;
using Core.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.ValueObjects;

namespace Volunteers.Infrastructure.BackgroundServices
{
    public class FilesCleanupBackgroundService : BackgroundService
    {
        private readonly ILogger<FilesCleanupBackgroundService> _logger;
        private readonly IMessageQueue<IEnumerable<FileStorageDeleteDto>> _messageQueue;
        private readonly IFilesProvider _fileProvider;

        public FilesCleanupBackgroundService(
            ILogger<FilesCleanupBackgroundService> logger,
            IMessageQueue<IEnumerable<FileStorageDeleteDto>> messageQueue,
            IFilesProvider fileProvider,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _messageQueue = messageQueue;
            _fileProvider = fileProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FilesCleanupBackgroundService is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                var fileSToDelete = await _messageQueue.ReadAsync(stoppingToken);

                var deleteResult = await _fileProvider.DeleteFiles(fileSToDelete, stoppingToken);
                if (deleteResult.IsFailure)
                    _logger.LogDebug("FilesCleanupBackgroundService: Failed to clean up MinIO files - {Errors}",
                        deleteResult.Error);
                else
                    _logger.LogInformation("FilesCleanupBackgroundService: Files deleted - {DeletedFiles}",
                        deleteResult.Value);
            }
        }
    }
}