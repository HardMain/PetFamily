using Core.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Tests.Infrastructure.Helpers;
using Volunteers.Application.Commands.AddPetFiles;

namespace Volunteers.IntegrationTests.Tests
{
    public class AddPetFilesHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand> _sut;

        public AddPetFilesHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
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

            var pet = await _volunteerReadDbContext.Pets
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

            var pet = await _volunteerReadDbContext.Pets
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

            var pet = await _volunteerReadDbContext.Pets
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

            var pet = await _volunteerReadDbContext.Pets
                .AsNoTracking()
                .FirstAsync();

            pet.Files.Should().BeEmpty();
        }
    }
}