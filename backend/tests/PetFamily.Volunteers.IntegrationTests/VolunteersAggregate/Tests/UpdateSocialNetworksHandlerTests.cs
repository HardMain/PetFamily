using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using FluentAssertions;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateSocialNetworks;
using PetFamily.Volunteers.IntegrationTests.VolunteersAggregate;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate.Tests
{
    public class UpdateSocialNetworksHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, UpdateSocialNetworksCommand> _sut;
        public UpdateSocialNetworksHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();
        }

        [Fact]
        public async Task Update_social_networks_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateSocialNetworksCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.SocialNetworks
                .Should()
                .BeEquivalentTo(command.Request.SocialNetworks);
        }

        [Fact]
        public async Task Update_social_networks_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateSocialNetworksCommand(VolunteerId.NewVolunteerId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.VolunteerId).ToErrorList());

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.SocialNetworks
                .Should()
                .NotBeEquivalentTo(command.Request.SocialNetworks);

        }
    }
}
