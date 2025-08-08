using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using FluentValidation;
using PetFamily.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteersHandler> _logger;

        public CreateVolunteersHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteersHandler> logger)
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

                return Errors.Volunteer.AlreadyExist().ToErrorList();
            }

            var email = Email.Create(command.Request.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteerByEmail.IsSuccess)
            {
                _logger.LogWarning(
                    "Volunteer creation failed: Email {Email} already exists", email.Value);

                return Errors.Volunteer.AlreadyExist().ToErrorList();
            }

            var name = FullName.Create(
                command.Request.FullName.firstName, 
                command.Request.FullName.lastName, 
                command.Request.FullName.middleName).Value;
               
            var description = command.Request.Description;

            var experienceYears = command.Request.ExperienceYears;

            var volunteerId = VolunteerId.NewVolunteerId(); 

            var volunteerResult = Volunteer.Create(
                volunteerId, 
                name, 
                email, 
                description, 
                experienceYears, 
                phoneNumber
            );

            if (volunteerResult.IsFailure) 
                return volunteerResult.Error.ToErrorList();

            await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Created volunteer with id {volunteerId}", volunteerId);

            return volunteerResult.Value.Id.Value; 
        }
    }
}