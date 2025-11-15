using FluentValidation;
using Framework.Validation;
using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<UpdateMainInfoCommand> validator,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
           UpdateMainInfoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get volunteer with {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var phoneNumber = PhoneNumber.Create(command.Request.PhoneNumber).Value;

            var volunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumber, cancellationToken);
            if (volunteerByPhone.IsSuccess && volunteerByPhone.Value.Id != command.VolunteerId)
            {
                _logger.LogWarning(
                    "Volunteer creation failed: Phone number {PhoneNumber} already exists", phoneNumber.Value);

                return Errors.Volunteer.Duplicate().ToErrorList();
            }

            var email = Email.Create(command.Request.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteerByEmail.IsSuccess && volunteerByEmail.Value.Id != command.VolunteerId)
            {
                _logger.LogWarning(
                    "Volunteer creation failed: Email {Email} already exists", email.Value);

                return Errors.Volunteer.Duplicate().ToErrorList();
            }

            var fullName = FullName.Create(
                command.Request.FullName.FirstName,
                command.Request.FullName.LastName,
                command.Request.FullName.MiddleName).Value;

            var description = command.Request.Description;
            var experienceYears = command.Request.ExperienceYears;

            volunteerResult.Value.UpdateMainInfo(fullName, email, phoneNumber, description, experienceYears);

            var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Main info updated for volunteer {volunteerId}", volunteerId);

            return result.Value;
        }
    }
}