using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Commands.Create;
using Volunteers.Domain.Entities;
using Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate.Tests
{
    public class CreateVolunteerHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;

        public CreateVolunteerHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
        }

        [Fact]
        public async Task Create_volunteer_successfuly_create_volunteer()
        {
            // Arrange
            var command = _fixture
                .CreateCreateVolunteerCommand();

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _volunteerReadDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            volunteer.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_some_volunteers_with_equal_email_and_phone_number_returns_error()
        {
            // Arrange
            var firstVolunteer = new Volunteer(
                VolunteerId.NewVolunteerId(),
                FullName.Create("firstName", "lastName", "middleName").Value,
                Email.Create("uniq_email@email.com").Value,
                "description",
                1,
                PhoneNumber.Create("+79999999999").Value);
            await _dataSeeder.InitVolunteer(firstVolunteer);

            var command = _fixture
                .CreateCreateVolunteerCommand();

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.Volunteer.Duplicate().ToErrorList());

            var countVolunteer = _volunteerReadDbContext.Volunteers.Count();

            countVolunteer.Should().Be(1);
        }
    }
}