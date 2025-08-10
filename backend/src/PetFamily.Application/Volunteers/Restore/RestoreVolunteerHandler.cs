using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.Volunteers.Restore
{
    public class RestoreVolunteerHandler
    {
        private readonly ILogger<RestoreVolunteerHandler> _logger;
        private readonly IValidator<RestoreVolunteerCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;

        public RestoreVolunteerHandler(
            ILogger<RestoreVolunteerHandler> logger,
            IValidator<RestoreVolunteerCommand> validator,
            IVolunteersRepository repository)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            RestoreVolunteerCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }
            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetByIdIncludingDeleted(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer with {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            volunteerResult.Value.Restore(true);

            var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Restored volunteer with id {volunteerId}", volunteerId);

            return result;
        }
    }
}
