using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfo;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersAggregate.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;
using PetFamily.Volunteers.IntegrationTests.Helpers;
using PetFamily.Volunteers.IntegrationTests.Helpers.Mappers;

namespace PetFamily.Volunteers.IntegrationTests.Volunteers.Tests
{
    public class UpdateMainInfoHandlerTests : VolunteerTestsBase
    {
        private readonly ICommandHandler<Guid, UpdateMainInfoCommand> _sut;
        public UpdateMainInfoHandlerTests(VolunteerTestsWebFactory factory) : base(factory)
        {
            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
        }

        [Fact]
        public async Task Update_main_info_successfuly_changes_order_in_database()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateMainInfoCommand(volunteerId);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            var mapped = VolunteerMapper.ToUpdateRequest(volunteer);
            mapped.Should().BeEquivalentTo(command.Request);
        }

        [Fact]
        public async Task Update_main_info_with_invalid_volunteer_id_returns_error()
        {
            // Arrange
            var volunteerId = await _dataSeeder.InitVolunteer();

            var command = _fixture
                .CreateUpdateMainInfoCommand(VolunteerId.NewVolunteerId());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.General.NotFound(command.VolunteerId).ToErrorList());

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            var mapped = VolunteerMapper.ToUpdateRequest(volunteer);
            mapped.Should().NotBeEquivalentTo(command.Request);

        }

        [Fact]
        public async Task Update_main_info_with_existing_number_phone_returns_error()
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

            var secondVolunteer = new Volunteer(
                VolunteerId.NewVolunteerId(),
                FullName.Create("firstName", "lastName", "middleName").Value,
                Email.Create("email@email.com").Value,
                "description",
                1,
                PhoneNumber.Create("+79999999998").Value);
            await _dataSeeder.InitVolunteer(secondVolunteer);

            var command = _fixture.CreateUpdateMainInfoCommand(secondVolunteer.Id);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.Volunteer.Duplicate().ToErrorList());

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            var mapped = VolunteerMapper.ToUpdateRequest(volunteer);
            mapped.Should().NotBeEquivalentTo(command.Request);
        }

        [Fact]
        public async Task Update_main_info_with_existing_email_returns_error()
        {
            // Arrange
            var firstVolunteer = new Volunteer(
                VolunteerId.NewVolunteerId(),
                FullName.Create("firstName", "lastName", "middleName").Value,
                Email.Create("email@email.com").Value,
                "description",
                1,
                PhoneNumber.Create("+79999999998").Value);
            await _dataSeeder.InitVolunteer(firstVolunteer);

            var secondVolunteer = new Volunteer(
                VolunteerId.NewVolunteerId(),
                FullName.Create("firstName", "lastName", "middleName").Value,
                Email.Create("email2@email.com").Value,
                "description",
                1,
                PhoneNumber.Create("+79999999998").Value);
            await _dataSeeder.InitVolunteer(secondVolunteer);

            var command = _fixture.CreateUpdateMainInfoCommand(secondVolunteer.Id);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeEquivalentTo(
                Errors.Volunteer.Duplicate().ToErrorList());

            var volunteer = await _readDbContext.Volunteers
                .AsNoTracking()
                .FirstAsync();

            var mapped = VolunteerMapper.ToUpdateRequest(volunteer);
            mapped.Should().NotBeEquivalentTo(command.Request);
        }
    }
}
