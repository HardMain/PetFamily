using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Configurations;

namespace PetFamily.Infrastructure.services
{
    public class SoftDeleteCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IOptions<SoftDeleteSettings> _options;
        private readonly ILogger<SoftDeleteCleanupService> _logger;

        public SoftDeleteCleanupService(
            IServiceProvider services,
            IOptions<SoftDeleteSettings> options,
            ILogger<SoftDeleteCleanupService> logger)
        {
            _services = services;
            _options = options;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();

                    var cutoffDate = DateTime.UtcNow.AddDays(_options.Value.RetentionDate * (-1));

                    var deletedIds = await repository.DeleteSoftDeletedEarlierThan(cutoffDate, cancellationToken);

                    if (deletedIds.Any())
                        _logger.LogInformation(
                            "BackgroundService: Deleted(hard) volunteers with ids {volunteerIds}", deletedIds);
                    else
                        _logger.LogDebug(
                            "BackgroundService: No volunteers for deletion(hard)");
                }
                    
                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
            }
        }
    }
}