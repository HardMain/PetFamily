using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdatePetSupportStatus;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Pets.Tests
{
    public class UpdatePetSupportStatusHandlerTests : PetTestsBase
    {
        private readonly ICommandHandler<Guid, UpdatePetSupportStatusCommand> _sut;
        public UpdatePetSupportStatusHandlerTests(PetTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdatePetSupportStatusCommand>>();
        }

        [Fact]
        public async Task Update_pet_support_status_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetSupportStatusCommand(volunteerId, petId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            pet.SupportStatus.Should().Be(command.Request.SupportStatus.ToString());
        }

        [Fact]
        public async Task Update_pet_support_status_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetSupportStatusCommand(VolunteerId.NewVolunteerId(), petId);

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

            pet.SupportStatus.Should().NotBe(command.Request.SupportStatus.ToString());
        }

        [Fact]
        public async Task Update_pet_support_status_with_invalid_pet_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetSupportStatusCommand(volunteerId, PetId.NewPetId());

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

            pet.SupportStatus.Should().NotBe(command.Request.SupportStatus.ToString());
        }
    }
}
