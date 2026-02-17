using Core.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Species.Contracts.DTOs;
using Tests.Infrastructure.Helpers;
using Volunteers.Application.Commands.AddPet;

namespace Volunteers.IntegrationTests.Tests;

public class AddPetHandlerTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, AddPetCommand> _sut;

    public AddPetHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
    }

    [Fact]
    public async Task Add_pet_to_database_successfuly_adds_pet()
    {
        // Arrange
        var volunteerId = await _dataSeeder.InitVolunteer();
        var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
        var command = _fixture
            .CreateAddPetCommand(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var pet = await _volunteerReadDbContext.Pets
            .AsNoTracking()
            .FirstAsync();

        pet.Should().NotBeNull();
        pet.VolunteerId.Should().Be(volunteerId);
    }

    [Fact]
    public async Task Add_pet_to_database_with_invalid_volunteer_id_returns_error()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId();
        var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
        var command = _fixture
            .CreateAddPetCommand(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error
            .Should()
            .BeEquivalentTo(
                Errors.General
                .NotFound(command.VolunteerId)
                .ToErrorList());

        var pet = await _volunteerReadDbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync();

        pet.Should().BeNull();
    }

    [Fact]
    public async Task Add_pet_to_database_with_invalid_species_id_and_breed_id_returns_error()
    {
        // Arrange
        var volunteerId = await _dataSeeder.InitVolunteer();
        var speciesAndBreed = new SpeciesAndBreedDto(SpeciesId.NewSpeciesId(), BreedId.NewBreedId());
        var command = _fixture
            .CreateAddPetCommand(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(
            Errors.SpeciesAndBreed
            .NotFound(speciesAndBreed.SpeciesId, speciesAndBreed.BreedId)
            .ToErrorList());

        var pet = await _volunteerReadDbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync();

        pet.Should().BeNull();
    }
}