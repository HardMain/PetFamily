using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Volunteers.Application.Commands.UpdateDonationsInfo;
using SharedKernel.ValueObjects.Ids;
using SharedKernel.Failures;
using Microsoft.EntityFrameworkCore;
using Core.Abstractions;
using Tests.Infrastructure.Helpers;

namespace Volunteers.IntegrationTests.Tests
{
    public class UpdateDonationsInfoHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, UpdateDonationsInfoCommand> _sut;
        public UpdateDonationsInfoHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateDonationsInfoCommand>>();
        }
        [Fact]
        public async Task Update_donations_info_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateDonationsInfoCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.DonationsInfo
                .Should()
                .BeEquivalentTo(command.Request.DonationsInfo);
        }

        [Fact]
        public async Task Update_donations_info_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateDonationsInfoCommand(VolunteerId.NewVolunteerId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.VolunteerId).ToErrorList());

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.DonationsInfo
                .Should()
                .NotBeEquivalentTo(command.Request.DonationsInfo);

        }
    }
}