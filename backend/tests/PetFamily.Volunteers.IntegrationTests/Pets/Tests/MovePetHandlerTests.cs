using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.MovePet;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Pets.Tests
{
    public class MovePetHandlerTests : PetTestsBase
    {
        private readonly ICommandHandler<Guid, MovePetCommand> _sut;

        public MovePetHandlerTests(PetTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, MovePetCommand>>();
        }

        [Fact]
        public async Task Move_pet_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);
            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var newPosition = 2;

            var command = _fixture
                .CreateMovePetCommand(volunteerId, petId, newPosition);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            pet.Position.Should().Be(2);
        }

        [Fact]
        public async Task Move_pet_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);
            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var newPosition = 2;

            var command = _fixture
                .CreateMovePetCommand(VolunteerId.NewVolunteerId(), petId, newPosition);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(command.VolunteerId)
                    .ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            pet.Position.Should().Be(1);
        }

        [Fact]
        public async Task Move_pet_with_invalid_pet_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);
            await _dataSeeder.InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var newPosition = 2;

            var command = _fixture
                .CreateMovePetCommand(volunteerId, PetId.NewPetId(), newPosition);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(command.PetId)
                    .ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            pet.Position.Should().Be(1);
        }
    }
}
