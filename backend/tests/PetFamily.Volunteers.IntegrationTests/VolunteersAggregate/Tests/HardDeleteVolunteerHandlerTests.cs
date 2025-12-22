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
    public class HardDeleteVolunteerHandlerTests : VolunteerTestsBase
    {

        private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
        public HardDeleteVolunteerHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<HardDeleteVolunteerHandler>();
        }

        [Fact]
        public async Task Hard_delete_volunteer_with_success_file_provider_successfuly_deletes_volunteer()
        {
            // Arrange
            _factory.SetupSuccessFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateDeleteVolunteerCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstOrDefaultAsync();

            volunteer.Should().BeNull();
        }

        [Fact]
        public async Task Hard_delete_volunteer_with_failed_file_provider_returns_error()
        {
            // Arrange
            _factory.SetupFailureFileProviderMock();
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateDeleteVolunteerCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.MinioProvider.FileDeleteError().ToErrorList());

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.Should().NotBeNull();
        }

        [Fact]
        public async Task Hard_delete_volunteer_with_failed_volunteer_id_returns_error()
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

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.Should().NotBeNull();
        }
    }
}
