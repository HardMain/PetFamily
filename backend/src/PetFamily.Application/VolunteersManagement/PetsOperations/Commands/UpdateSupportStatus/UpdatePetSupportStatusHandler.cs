using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.UpdateSupportStatus
{
    public class UpdatePetSupportStatusHandler : ICommandHandler<Guid, UpdatePetSupportStatusCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdatePetSupportStatusHandler> _logger;
        private readonly IValidator<UpdatePetSupportStatusCommand> _validator;

        public UpdatePetSupportStatusHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdatePetSupportStatusHandler> logger,
            IValidator<UpdatePetSupportStatusCommand> validator)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetSupportStatusCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer {VolunteerId}", command.VolunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning("Failed to get pet {PetId}", command.PetId);

                return petResult.Error.ToErrorList();
            }

            var petSupportStatus = command.Request.SupportStatus;
            if (!Enum.IsDefined(typeof(PetSupportStatusDto), command.Request.SupportStatus))
            {
                _logger.LogWarning(
                    "Invalid support status {StatusValue} for pet {PetId}",
                    command.Request.SupportStatus,
                    petId);

                return Errors.General.ValueIsInvalid("supportStatus").ToErrorList();
            }

            volunteerResult.Value.UpdatePetSupportStatus(petResult.Value, (SupportStatus)petSupportStatus);

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = petResult.Value.Id.Value;

            _logger.LogWarning("PetSupportStatus updated {PetId}", result);

            return result;
        }
    }
}