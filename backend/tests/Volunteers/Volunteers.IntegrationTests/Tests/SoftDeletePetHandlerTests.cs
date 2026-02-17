using Core.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Tests.Infrastructure.Helpers;
using Volunteers.Application.Commands.DeletePet;

namespace Volunteers.IntegrationTests.Tests
{
    public class SoftDeletePetHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, DeletePetCommand> _sut;
        public SoftDeletePetHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<SoftDeletePetHandler>();
        }

        [Fact]
        public async Task Delete_pet_from_database_successfuly_deletes_pet()
        {
            // Arrange
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

            var pet = (await _volunteerWriteDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.Should().NotBeNull();
            pet.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_pet_from_database_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
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

            var pet = (await _volunteerWriteDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.Should().NotBeNull();
            pet.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_pet_from_database_with_invalid_pet_id_returns_error()
        {
            // Arrange
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

            var pet = (await _volunteerWriteDbContext.Volunteers
                .Include(v => v.Pets)
                .FirstAsync(v => v.Id == volunteerId))
                .Pets.First();

            pet.Should().NotBeNull();
            pet.IsDeleted.Should().BeFalse();
        }
    }
}
