using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.SpeciesManagement.Commands.Create;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Species.Tests
{
    public class CreateSpeciesHandlerTests : SpeciesTestsBase
    {
        private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;

        public CreateSpeciesHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
        }

        [Fact]
        public async Task Add_breed_to_database_successfuly_adds_breed()
        {
            // Arrange
            var command = _fixture
                .CreateCreateSpeciesCommand();

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var species = await _readDbContext.Species
                .AsNoTracking()
                .FirstAsync();

            species.Should().NotBeNull();
            species.Id.Should().Be(result.Value);
        }
    }
}
