using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using FluentValidation;
using PetFamily.Application.Extensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;
using PetFamily.Domain.VolunteersAggregate.Entities;

namespace PetFamily.Application.VolunteersAggregate.Commands.Create
{
    public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public CreateVolunteerHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var phoneNumber = PhoneNumber.Create(command.Request.PhoneNumber).Value;

            var volunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumber, cancellationToken);
            if (volunteerByPhone.IsSuccess)
            {
                _logger.LogWarning(
                    "Volunteer creation failed: Phone number {PhoneNumber} already exists", phoneNumber.Value);

                return Errors.Volunteer.Duplicate().ToErrorList();
            }

            var email = Email.Create(command.Request.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteerByEmail.IsSuccess)
            {
                _logger.LogWarning(
                    "Volunteer creation failed: Email {Email} already exists", email.Value);

                return Errors.Volunteer.Duplicate().ToErrorList();
            }

            var name = FullName.Create(
                command.Request.FullName.FirstName,
                command.Request.FullName.LastName,
                command.Request.FullName.MiddleName).Value;

            var description = command.Request.Description;

            var experienceYears = command.Request.ExperienceYears;

            var volunteerId = VolunteerId.NewVolunteerId();

            var volunteer = new Volunteer(
                volunteerId,
                name,
                email,
                description,
                experienceYears,
                phoneNumber
            );

            if (command.Request.DonationsInfo is not null)
            {
                var donationsInfo = ListDonationInfo.Create(command.Request.DonationsInfo
                    .Select(di => DonationInfo.Create(di.Title, di.Description).Value));

                volunteer.SetListDonationInfo(donationsInfo.Value);
            }

            if (command.Request.SocialNetworks is not null)
            {
                var socialNetworks = ListSocialNetwork.Create(command.Request.SocialNetworks
                    .Select(di => SocialNetwork.Create(di.URL, di.Platform).Value));

                volunteer.SetListSocialNetwork(socialNetworks.Value);
            }

            var result = await _volunteersRepository.Add(volunteer, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Volunteer {volunteerId} created", volunteerId);

            return result.Value;
        }
    }
}