using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Delete;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Restore;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Pets.Tests
{
    public class RestorePetHandlerTests : PetTestsBase
    {
        private readonly ICommandHandler<Guid, RestorePetCommand> _sutPetRestore;
        private readonly ICommandHandler<Guid, DeletePetCommand> _sutPetDelete;
        public RestorePetHandlerTests(PetTestsWebFactory factory) : base(factory)
        {
            _sutPetRestore = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, RestorePetCommand>>();

            _sutPetDelete = _scope.ServiceProvider
                .GetRequiredService<SoftDeletePetHandler>();
        }

        [Fact]
        public async Task Restore_pet_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var deleteCommand = _fixture
                .CreateDeletePetCommand(volunteerId, petId);
            var deleteResult = await _sutPetDelete.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsSuccess.Should().BeTrue();

            var restoreCommand = _fixture
                .CreateRestorePetCommand(volunteerId, petId);

            // Act
            var result = await _sutPetRestore.Handle(restoreCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = (await _writeDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task Restore_pet_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var deleteCommand = _fixture
                .CreateDeletePetCommand(volunteerId, petId);
            var deleteResult = await _sutPetDelete.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsSuccess.Should().BeTrue();

            var restoreCommand = _fixture
                .CreateRestorePetCommand(VolunteerId.NewVolunteerId(), petId);

            // Act
            var result = await _sutPetRestore.Handle(restoreCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(restoreCommand.VolunteerId)
                    .ToErrorList());

            var pet = (await _writeDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Restore_pet_with_invalid_pet_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var deleteCommand = _fixture
                .CreateDeletePetCommand(volunteerId, petId);
            var deleteResult = await _sutPetDelete.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsSuccess.Should().BeTrue();

            var restoreCommand = _fixture
                .CreateRestorePetCommand(volunteerId, PetId.NewPetId());

            // Act
            var result = await _sutPetRestore.Handle(restoreCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(restoreCommand.PetId)
                    .ToErrorList());

            var pet = (await _writeDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.IsDeleted.Should().BeTrue();
        }
    }
}
