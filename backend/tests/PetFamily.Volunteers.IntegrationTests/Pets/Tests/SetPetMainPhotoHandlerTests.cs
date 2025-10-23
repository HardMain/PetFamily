using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.AddPetFiles;
using PetFamily.Application.Volunteers.Commands.SetMainPhotoPet;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Pets.Tests
{
    public class SetPetMainPhotoHandlerTests : PetTestsBase
    {
        private readonly ICommandHandler<string, SetPetMainPhotoCommand> _sutSetMainPhoto;
        private readonly ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand> _sutAddFiles;
        public SetPetMainPhotoHandlerTests(PetTestsWebFactory factory) : base(factory)
        {
            _sutSetMainPhoto = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<string, SetPetMainPhotoCommand>>();

            _sutAddFiles = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand>>();
        }

        [Fact]
        public async Task Set_pet_main_photo_successfuly_changes_order_in_database()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addFilesCommand = _fixture
                .CreateAddPetFilesCommand(volunteerId, petId);
            var addFilesResult = await _sutAddFiles.Handle(addFilesCommand, CancellationToken.None);

            addFilesResult.IsSuccess.Should().BeTrue();

            var petBefore = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var newMainPhoto = petBefore.Files.First().PathToStorage;

            var setMainPhotoCommand = _fixture
                .CreateSetPetMainPhotoCommand(volunteerId, petId, newMainPhoto);

            // Act
            var result = await _sutSetMainPhoto.Handle(setMainPhotoCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Should().NotBeNull();
            pet.MainPhoto.Should().NotBeNull();
        }

        [Fact]
        public async Task Set_pet_main_photo_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addFilesCommand = _fixture
                .CreateAddPetFilesCommand(volunteerId, petId);
            var addFilesResult = await _sutAddFiles.Handle(addFilesCommand, CancellationToken.None);

            addFilesResult.IsSuccess.Should().BeTrue();

            var petBefore = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var newMainPhoto = petBefore.Files.First().PathToStorage;

            var setMainPhotoCommand = _fixture
                .CreateSetPetMainPhotoCommand(VolunteerId.NewVolunteerId(), petId, newMainPhoto);

            // Act
            var result = await _sutSetMainPhoto.Handle(setMainPhotoCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
            .Should()
            .BeEquivalentTo(
                Errors.General
                .NotFound(setMainPhotoCommand.VolunteerId)
                .ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.MainPhoto.Should().BeNull();
        }

        [Fact]
        public async Task Set_pet_main_photo_with_invalid_pet_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addFilesCommand = _fixture
                .CreateAddPetFilesCommand(volunteerId, petId);
            var addFilesResult = await _sutAddFiles.Handle(addFilesCommand, CancellationToken.None);

            addFilesResult.IsSuccess.Should().BeTrue();

            var petBefore = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var newMainPhoto = petBefore.Files.First().PathToStorage;

            var setMainPhotoCommand = _fixture
                .CreateSetPetMainPhotoCommand(volunteerId, PetId.NewPetId(), newMainPhoto);

            // Act
            var result = await _sutSetMainPhoto.Handle(setMainPhotoCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
            .Should()
            .BeEquivalentTo(
                Errors.General
                .NotFound(setMainPhotoCommand.PetId)
                .ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.MainPhoto.Should().BeNull();
        }
    }
}
