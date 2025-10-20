using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.FilesOperations.AddPetFiles;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;

namespace PetFamily.Volunteers.IntegrationTests.Pets.Tests
{
    public class AddPetFilesHandlerTests : PetTestsBase
    {
        private readonly ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand> _sut;

        public AddPetFilesHandlerTests(PetTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand>>();
        }

        [Fact]
        public async Task Add_files_to_database_with_success_file_provider_successfuly_adds_files()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateAddPetFilesCommand(volunteerId, petId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Files.Should().NotBeNull();
            pet.Files.Should().HaveCountGreaterThanOrEqualTo(4);
        }

        [Fact]
        public async Task Add_files_to_database_with_failed_file_provider_returns_error()
        {
            // Arrange
            _factory.SetupFailureFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateAddPetFilesCommand(volunteerId, petId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.MinioProvider.FileUploadError().ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Files.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_files_to_database_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateAddPetFilesCommand(VolunteerId.NewVolunteerId(), petId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.VolunteerId).ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Files.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_files_to_database_with_invalid_pet_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateAddPetFilesCommand(volunteerId, PetId.NewPetId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.PetId).ToErrorList());

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Files.Should().BeEmpty();
        }
    }
}