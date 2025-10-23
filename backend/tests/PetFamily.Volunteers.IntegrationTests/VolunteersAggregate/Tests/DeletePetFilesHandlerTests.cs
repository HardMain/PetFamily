using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersAggregate.Commands.AddPetFiles;
using PetFamily.Application.VolunteersAggregate.Commands.DeletePetFiles;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate.Tests
{
    public class DeletePetFilesHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand> _sutAddFiles;
        private readonly ICommandHandler<IReadOnlyList<string>, DeletePetFilesCommand> _sutDeleteFiles;

        public DeletePetFilesHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sutAddFiles = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<IReadOnlyList<string>, AddPetFilesCommand>>();

            _sutDeleteFiles = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<IReadOnlyList<string>, DeletePetFilesCommand>>();
        }

        [Fact]
        public async Task Delete_files_from_database_with_success_file_provider_successfuly_deletes_files()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addCommand = _fixture.CreateAddPetFilesCommand(volunteerId, petId);
            var addResult = await _sutAddFiles.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();

            var petBefore = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var fileNames = petBefore.Files.Select(f => f.PathToStorage).ToList();

            var deleteCommand = _fixture.CreateDeletePetFilesCommand(volunteerId, petId, fileNames);

            // Act
            var result = await _sutDeleteFiles.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var petAfter = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            petAfter.Files.Should().BeEmpty();
        }

        [Fact]
        public async Task Delete_files_from_database_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addCommand = _fixture.CreateAddPetFilesCommand(volunteerId, petId);
            var addResult = await _sutAddFiles.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var fileNames = pet.Files.Select(f => f.PathToStorage).ToList();

            var deleteCommand = _fixture
                .CreateDeletePetFilesCommand(VolunteerId.NewVolunteerId(), petId, fileNames);

            // Act
            var result = await _sutDeleteFiles.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(deleteCommand.VolunteerId)
                    .ToErrorList());

            pet.Files.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Delete_files_from_database_with_invalid_pet_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var addCommand = _fixture.CreateAddPetFilesCommand(volunteerId, petId);
            var addResult = await _sutAddFiles.Handle(addCommand, CancellationToken.None);

            addResult.IsSuccess.Should().BeTrue();

            var pet = await _readDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);
            var fileNames = pet.Files.Select(f => f.PathToStorage).ToList();

            var deleteCommand = _fixture
                .CreateDeletePetFilesCommand(volunteerId, PetId.NewPetId(), fileNames);

            // Act
            var result = await _sutDeleteFiles.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(deleteCommand.PetId)
                    .ToErrorList());

            pet.Files.Should().NotBeEmpty();
        }
    }
}
