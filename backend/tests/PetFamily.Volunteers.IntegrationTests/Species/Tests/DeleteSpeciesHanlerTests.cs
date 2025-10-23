using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Species.Commands.Delete;
using PetFamily.Application.SpeciesManagement.Commands.Create;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;

namespace PetFamily.Volunteers.IntegrationTests.Species.Tests
{
    public class DeleteSpeciesHanlerTests : SpeciesTestsBase
    {
        private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;

        public DeleteSpeciesHanlerTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
        }

        [Fact]
        public async Task Delete_species_from_database_successfuly_deletes_species()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();

            var command = _fixture.CreateDeleteSpeciesCommand(speciesId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var species = await _readDbContext.Species
                .AsNoTracking()
                .FirstOrDefaultAsync();

            species.Should().BeNull();
        }

        [Fact]
        public async Task Delete_species_from_database_with_failed_species_id_returns_error()
        {
            // Arrange
            var speciesId = await _dataSeeder.InitSpecies();

            var command = _fixture.CreateDeleteSpeciesCommand(SpeciesId.NewSpeciesId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(Errors.General.NotFound(command.Id).ToErrorList());

            var species = await _readDbContext.Species
                .AsNoTracking()
                .FirstOrDefaultAsync();

            species.Should().NotBeNull();
        }
    }
}