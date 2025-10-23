using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Species.Commands.AddBreed;
using PetFamily.Application.Species.Commands.DeleteBreed;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Breeds.Tests
{
    public class DeleteBreedHandlerTests : BreedTestsBase
    {
        private readonly ICommandHandler<Guid, AddBreedCommand> _sutAddBreedCommand;
        private readonly ICommandHandler<Guid, DeleteBreedCommand> _sutDeleteBreedCommand;

        public DeleteBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sutAddBreedCommand = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();

            _sutDeleteBreedCommand = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
        }

        [Fact]
        public async Task Delete_breed_from_database_successfuly_deletes_breed()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();

            var addCommand = _fixture
                .CreateAddBreedCommand(speciesId);
            
            var addResult = await _sutAddBreedCommand.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();
            addResult.Value.Should().NotBeEmpty();

            var deleteCommand = _fixture
                .CreateDeleteBreedCommand(speciesId, addResult.Value);

            // Act
            var result = await _sutDeleteBreedCommand.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var breed = await _readDbContext.Breeds
                .AsNoTracking()
                .FirstOrDefaultAsync();

            breed.Should().BeNull();
        }

        [Fact]
        public async Task Delete_breed_from_database_with_failed_species_id_returns_error()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();

            var addCommand = _fixture
                .CreateAddBreedCommand(speciesId);

            var addResult = await _sutAddBreedCommand.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();
            addResult.Value.Should().NotBeEmpty();

            var deleteCommand = _fixture
                .CreateDeleteBreedCommand(SpeciesId.NewSpeciesId(), addResult.Value);

            // Act
            var result = await _sutDeleteBreedCommand.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(Errors.General.NotFound(deleteCommand.SpeciesId).ToErrorList());

            var breed = await _readDbContext.Breeds
                .AsNoTracking()
                .FirstOrDefaultAsync();

            breed.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_breed_from_database_with_failed_breed_id_returns_error()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();

            var addCommand = _fixture
                .CreateAddBreedCommand(speciesId);

            var addResult = await _sutAddBreedCommand.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();
            addResult.Value.Should().NotBeEmpty();

            var deleteCommand = _fixture
                .CreateDeleteBreedCommand(speciesId, BreedId.NewBreedId());

            // Act
            var result = await _sutDeleteBreedCommand.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(Errors.General.NotFound(deleteCommand.BreedId).ToErrorList());

            var breed = await _readDbContext.Breeds
                .AsNoTracking()
                .FirstOrDefaultAsync();

            breed.Should().NotBeNull();
        }
    }
}
