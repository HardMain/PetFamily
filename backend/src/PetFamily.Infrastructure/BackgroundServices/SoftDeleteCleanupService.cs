using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Application.VolunteersOperations;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public class SoftDeleteCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IOptions<SoftDeleteOptions> _options;
        private readonly ILogger<SoftDeleteCleanupService> _logger;

        public SoftDeleteCleanupService(
            IServiceProvider services,
            IOptions<SoftDeleteOptions> options,
            ILogger<SoftDeleteCleanupService> logger)
        {
            _services = services;
            _options = options;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SoftDeleteCleanupService is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _services.CreateAsyncScope();

                var repository = scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();

                var cutoffDate = DateTime.UtcNow.AddDays(_options.Value.RetentionDate * -1);
                //var cutoffDate = DateTime.UtcNow.AddSeconds(50 * (-1));

                var countDeleted = await repository.DeleteSoftDeletedEarlierThan(cutoffDate, stoppingToken);

                if (countDeleted > 0)
                    _logger.LogInformation(
                        "SoftDeleteCleanupService: Deleted(Hard) {CountDeleted} volunteers", countDeleted);
                else
                    _logger.LogDebug(
                        "SoftDeleteCleanupService: No volunteers for deletion(Hard)");

                await Task.Delay(/*TimeSpan.FromSeconds(30)*/TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}