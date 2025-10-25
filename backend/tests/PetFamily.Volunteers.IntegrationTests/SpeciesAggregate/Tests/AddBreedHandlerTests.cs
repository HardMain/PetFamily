using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.SpeciesAggregate.Commands.AddBreed;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.SpeciesAggregate.Tests
{
    public class AddBreedHandlerTests : SpeciesTestsBase
    {
        private readonly ICommandHandler<Guid, AddBreedCommand> _sut;

        public AddBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
        }

        [Fact]
        public async Task Add_breed_to_database_successfuly_adds_breed()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();
            var command = _fixture
                .CreateAddBreedCommand(speciesId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var breed = await _readDbContext.Breeds
                .AsNoTracking()
                .FirstAsync();

            breed.Should().NotBeNull();
            breed.Id.Should().Be(result.Value);
        }

        [Fact]
        public async Task Add_breed_to_database_with_failed_species_id_returns_error()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();
            var command = _fixture
                .CreateAddBreedCommand(SpeciesId.NewSpeciesId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(Errors.General.NotFound(command.SpeciesId).ToErrorList());

            var breed = await _readDbContext.Breeds
                .AsNoTracking()
                .FirstOrDefaultAsync();

            breed.Should().BeNull();
        }
    }
}