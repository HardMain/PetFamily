using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.Volunteers.IntegrationTests
{
    public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithDatabase("pet_family_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        private Respawner _respawner = default!;
        private DbConnection _dbConnection = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
        }

        protected virtual void ConfigureDefaultServices(IServiceCollection services)
        {
            var readContext = services
                .SingleOrDefault(s => s.ServiceType == typeof(IReadDbContext));

            var writeContext = services
                .SingleOrDefault(s => s.ServiceType == typeof(WriteDbContext));

            if (readContext is not null)
                services.Remove(readContext);

            if (writeContext is not null)
                services.Remove(writeContext);

            services.AddScoped<IReadDbContext>(
                _ => new ReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(
                _ => new WriteDbContext(_dbContainer.GetConnectionString()));
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
    }
}
