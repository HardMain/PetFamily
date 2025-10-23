using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using PetFamily.Application.VolunteersAggregate.Commands.Create;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;
using PetFamily.Domain.VolunteersAggregate.Entities;

namespace PetFamily.Volunteers.IntegrationTests.Volunteers.Tests
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

            var volunteer = await _readDbContext.Volunteers
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

            var countVolunteer = _readDbContext.Volunteers.Count();

            countVolunteer.Should().Be(1);
        }
    }
}