using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Species.Application.Abstractions;
using Species.Infrastructure.DbContexts;
using Tests.Infrastructure.Helpers;
using Volunteers.Application.Abstractions;
using Volunteers.Infrastructure.DbContexts;

namespace Volunteers.IntegrationTests
{
    public class VolunteerTestsBase : IClassFixture<VolunteerTestsWebFactory>, IAsyncLifetime
    {
        protected readonly VolunteerTestsWebFactory _factory;
        protected readonly Fixture _fixture;
        protected readonly IServiceScope _scope;
        protected readonly IVolunteersReadDbContext _volunteerReadDbContext;
        protected readonly VolunteersWriteDbContext _volunteerWriteDbContext;
        protected readonly ISpeciesReadDbContext _speciesReadDbContext;
        protected readonly SpeciesWriteDbContext _speciesWriteDbContext;
        protected readonly TestDataSeeder _dataSeeder;
        public VolunteerTestsBase(VolunteerTestsWebFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _scope = factory.Services.CreateAsyncScope();
            
            _volunteerReadDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
            _volunteerWriteDbContext = _scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();

            _speciesReadDbContext = _scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
            _speciesWriteDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();

            _dataSeeder = new TestDataSeeder(_volunteerWriteDbContext, _speciesWriteDbContext);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            _scope.Dispose();
            await _factory.ResetDatabaseAsync();
        }
    }
}