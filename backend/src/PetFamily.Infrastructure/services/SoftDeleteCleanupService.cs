using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.Services
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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();

                    var cutoffDate = DateTime.UtcNow.AddDays(_options.Value.RetentionDate * (-1));
                    //var cutoffDate = DateTime.UtcNow.AddSeconds(50 * (-1));

                    var countDeleted = await repository.DeleteSoftDeletedEarlierThan(cutoffDate, cancellationToken);

                    if (countDeleted > 0)
                        _logger.LogInformation(
                            "BackgroundService: Deleted(hard) {countDeleted} volunteers", countDeleted);
                    else
                        _logger.LogDebug(
                            "BackgroundService: No volunteers for deletion(hard)");
                }
                    
                await Task.Delay(/*TimeSpan.FromSeconds(30)*/TimeSpan.FromHours(24), cancellationToken);
            }
        }
    }
}