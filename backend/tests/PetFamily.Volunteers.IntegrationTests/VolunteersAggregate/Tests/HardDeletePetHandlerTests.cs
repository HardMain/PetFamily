using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersAggregate.Commands.DeletePet;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate.Tests
{
    public class HardDeletePetHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, DeletePetCommand> _sut;

        public HardDeletePetHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<HardDeletePetHandler>();
        }

        [Fact]
        public async Task Delete_pet_from_database_successfuly_deletes_pet()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateDeletePetCommand(volunteerId, petId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstOrDefaultAsync();

            pet.Should().BeNull();
        }

        [Fact]
        public async Task Delete_pet_from_database_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateDeletePetCommand(VolunteerId.NewVolunteerId(), petId);

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
                .FirstOrDefaultAsync();

            pet.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_pet_from_database_with_invalid_pet_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateDeletePetCommand(volunteerId, PetId.NewPetId());

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
                .FirstOrDefaultAsync();

            pet.Should().NotBeNull();
        }
    }
}
