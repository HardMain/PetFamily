using Core.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Tests.Infrastructure.Helpers;
using Tests.Infrastructure.Helpers.Mappers;
using Volunteers.Application.Commands.UpdateMainInfoPet;

namespace Volunteers.IntegrationTests.Tests
{
    public class UpdatePetHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, UpdatePetCommand> _sut;
        public UpdatePetHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();
        }

        [Fact]
        public async Task Update_pet_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetCommand(volunteerId, petId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await _volunteerReadDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            var mapped = PetMapper.ToUpdateRequest(pet);
            mapped.Should()
                .BeEquivalentTo(command.Request, options =>
                options
                    .ExcludingMissingMembers()
                    .Using<DateTime>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(10)))
                    .WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task Update_pet_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetCommand(VolunteerId.NewVolunteerId(), petId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(command.VolunteerId)
                    .ToErrorList());

            var pet = await _volunteerReadDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            var mapped = PetMapper.ToUpdateRequest(pet);
            mapped.Should()
                .NotBeEquivalentTo(command.Request, options =>
                options
                    .ExcludingMissingMembers()
                    .Using<DateTime>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(10)))
                    .WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task Update_pet_with_invalid_pet_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();
            var speciesAndBreed = await _dataSeeder.InitSpeciesAndBreed();
            var petId = await _dataSeeder
                .InitPet(volunteerId, speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            var command = _fixture
                .CreateUpdatePetCommand(volunteerId, PetId.NewPetId(), speciesAndBreed.SpeciesId, speciesAndBreed.BreedId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(command.PetId)
                    .ToErrorList());

            var pet = await _volunteerReadDbContext.Pets
                .AsNoTracking()
                .FirstAsync(p => p.Id == petId);

            var mapped = PetMapper.ToUpdateRequest(pet);
            mapped.Should()
                .NotBeEquivalentTo(command.Request, options =>
                options
                    .ExcludingMissingMembers()
                    .Using<DateTime>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(10)))
                    .WhenTypeIs<DateTime>());
        }
    }
}
