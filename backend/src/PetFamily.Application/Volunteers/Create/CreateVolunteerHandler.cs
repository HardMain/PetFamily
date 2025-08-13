using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using FluentValidation;
using PetFamily.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace PetFamily.Application.Volunteers.Create
{
    public class CreateVolunteerHandler
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
                command.Request.FullName.firstName,
                command.Request.FullName.lastName,
                command.Request.FullName.middleName).Value;

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

            var donationInfos = command.Request.DonationsInfo?
                .Select(di => DonationInfo.Create(di.Title, di.Description).Value) ?? [];

            var errorsAddDonationInfos = volunteer.AddDonationsInfo(donationInfos);
            if (errorsAddDonationInfos.Any())
            {
                _logger.LogWarning(
                    "Failed to add donation infos: {Errors}", errorsAddDonationInfos);

                return errorsAddDonationInfos;
            }

            var socialNetworks = command.Request.SocialNetworks?
                .Select(sn => SocialNetwork.Create(sn.URL, sn.Platform).Value) ?? [];
            
            var errorsAddSocialNetworks = volunteer.AddSocialNetworks(socialNetworks);
            if (errorsAddSocialNetworks.Any())
            {
                _logger.LogWarning(
                    "Failed to add social networks: {Errors}", errorsAddSocialNetworks);

                return errorsAddSocialNetworks;
            }

            var result = await _volunteersRepository.Add(volunteer, cancellationToken);

            _logger.LogInformation("Created volunteer with id {volunteerId}", volunteerId);

            return result;
        }
    }
}