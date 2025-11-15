using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Commands.Delete;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate.Tests
{
    public class SoftDeleteVolunteerHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
        public SoftDeleteVolunteerHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<SoftDeleteVolunteerHandler>();
        }

        [Fact]
        public async Task Soft_delete_volunteer_successfuly_changes_deletion_status()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateDeleteVolunteerCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _volunteerWriteDbContext.Volunteers
                .IgnoreQueryFilters()
                .FirstAsync();

            volunteer.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Soft_delete_volunteer_with_failed_volunteer_id_returns_error()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateDeleteVolunteerCommand(VolunteerId.NewVolunteerId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.VolunteerId).ToErrorList());

            var volunteer = await _volunteerWriteDbContext.Volunteers
                .IgnoreQueryFilters()
                .FirstAsync();

            volunteer.IsDeleted.Should().BeFalse();
        }
    }
}
