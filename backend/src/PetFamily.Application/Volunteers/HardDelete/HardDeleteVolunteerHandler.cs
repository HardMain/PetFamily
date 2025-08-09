using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Volunteers.HardDelete
{
    public class HardDeleteVolunteerHandler
    {
        private readonly ILogger<HardDeleteVolunteerHandler> _logger;
        private readonly IValidator<HardDeleteVolunteerCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;

        public HardDeleteVolunteerHandler(
            ILogger<HardDeleteVolunteerHandler> logger,
            IValidator<HardDeleteVolunteerCommand> validator,
            IVolunteersRepository repository)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            HardDeleteVolunteerCommand command, CancellationToken cancellationToken)
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
                _logger.LogWarning("Failed to get volunteer with {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var result = await _volunteersRepository.Delete(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Deleted(hard) volunteer with id {volunteerId}", volunteerId);

            return result;
        }
    }
}