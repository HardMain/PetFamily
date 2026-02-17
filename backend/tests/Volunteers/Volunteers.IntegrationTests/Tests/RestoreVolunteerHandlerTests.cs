using Core.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Tests.Infrastructure.Helpers;
using Volunteers.Application.Commands.Delete;
using Volunteers.Application.Commands.Restore;

namespace Volunteers.IntegrationTests.Tests
{
    public class RestoreVolunteerHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, RestoreVolunteerCommand> _sutVolunteerRestore;
        private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sutVolunteerDelete;
        public RestoreVolunteerHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sutVolunteerRestore = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, RestoreVolunteerCommand>>();

            _sutVolunteerDelete = _scope.ServiceProvider
                .GetRequiredService<SoftDeleteVolunteerHandler>();
        }

        [Fact]
        public async Task Restore_volunteer_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var deleteCommand = _fixture
                .CreateDeleteVolunteerCommand(volunteerId);
            var deleteResult = await _sutVolunteerDelete.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsSuccess.Should().BeTrue();

            var restoreCommand = _fixture
                .CreateRestoreVolunteerCommand(volunteerId);

            // Act
            var result = await _sutVolunteerRestore.Handle(restoreCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _volunteerWriteDbContext.Volunteers
                .IgnoreQueryFilters()
                .FirstAsync();

            volunteer.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task Restore_volunteer_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var deleteCommand = _fixture
                .CreateDeleteVolunteerCommand(volunteerId);
            var deleteResult = await _sutVolunteerDelete.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsSuccess.Should().BeTrue();

            var restoreCommand = _fixture
                .CreateRestoreVolunteerCommand(VolunteerId.NewVolunteerId());

            // Act
            var result = await _sutVolunteerRestore.Handle(restoreCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error
                .Should()
                .BeEquivalentTo(
                    Errors.General.NotFound(restoreCommand.VolunteerId)
                    .ToErrorList());

            var volunteer = await _volunteerWriteDbContext.Volunteers
                .IgnoreQueryFilters()
                .FirstAsync();

            volunteer.IsDeleted.Should().BeTrue();
        }
    }
}
