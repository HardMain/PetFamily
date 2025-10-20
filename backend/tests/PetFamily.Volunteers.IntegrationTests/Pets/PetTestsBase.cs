using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Pets
{
    public class PetTestsBase : IClassFixture<PetTestsWebFactory>, IAsyncLifetime
    {
        protected readonly PetTestsWebFactory _factory;
        protected readonly Fixture _fixture;
        protected readonly IServiceScope _scope;
        protected readonly IReadDbContext _readDbContext;
        protected readonly WriteDbContext _writeDbContext;
        protected readonly TestDataSeeder _dataSeeder;

        public PetTestsBase(PetTestsWebFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _scope = factory.Services.CreateAsyncScope();
            _readDbContext = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
            _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            _dataSeeder = new TestDataSeeder(_writeDbContext);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            _scope.Dispose();
            await _factory.ResetDatabaseAsync();
        }
    }
}