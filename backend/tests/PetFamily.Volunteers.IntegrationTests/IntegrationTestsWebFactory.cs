using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Species.Application.Abstractions;
using Species.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;
using Volunteers.Application.Abstractions;
using Volunteers.Infrastructure.DbContexts;

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
            RemoveDecriptor(services, typeof(IVolunteersReadDbContext));
            RemoveDecriptor(services, typeof(VolunteersWriteDbContext));
            RemoveDecriptor(services, typeof(ISpeciesReadDbContext));
            RemoveDecriptor(services, typeof(SpeciesWriteDbContext));

            var connectionString = _dbContainer.GetConnectionString();

            services.AddDbContext<VolunteersReadDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<SpeciesReadDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            
            services.AddDbContext<VolunteersWriteDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddDbContext<SpeciesWriteDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IVolunteersReadDbContext>(sp =>
                sp.GetRequiredService<VolunteersReadDbContext>());

            services.AddScoped<ISpeciesReadDbContext>(sp =>
                sp.GetRequiredService<SpeciesReadDbContext>());

        }

        private static void RemoveDecriptor(IServiceCollection services, Type serviceType)
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == serviceType);
            if (descriptor is not null)
                services.Remove(descriptor);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();
            var VolunteerdbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
            await VolunteerdbContext.Database.EnsureCreatedAsync();

            var SpeciesdbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            await SpeciesdbContext.Database.EnsureCreatedAsync();

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
