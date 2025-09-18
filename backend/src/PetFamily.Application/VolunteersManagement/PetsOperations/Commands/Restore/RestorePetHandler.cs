using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Application.VolunteersManagement.Commands.Restore;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Restore
{
    public class RestorePetHandler : ICommandHandler<Guid, RestorePetCommand>
    {
        private readonly ILogger<RestoreVolunteerHandler> _logger;
        private readonly IValidator<RestorePetCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;

        public RestorePetHandler(
            ILogger<RestoreVolunteerHandler> logger,
            IValidator<RestorePetCommand> validator,
            IVolunteersRepository repository)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            RestorePetCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }
            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetByIdIncludingSoftDeleted(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer with {volunteerId}", volunteerId);
                return volunteerResult.Error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get pet {PetId}: {Errors}",
                    petId,
                    petResult.Error);

                return petResult.Error.ToErrorList();
            }

            volunteerResult.Value.RestorePet(petResult.Value, true);

            var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Pet {PetId} restored", petId);

            return result.Value;
        }
    }
}
