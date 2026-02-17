using Microsoft.Extensions.Logging;
using Volunteers.Application.Commands.Restore;
using Volunteers.Application.Abstractions;
using FluentValidation;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Core.Extensions;
using Core.Abstractions;

namespace Volunteers.Application.Commands.RestorePet
{
    public class RestorePetHandler : ICommandHandler<Guid, RestorePetCommand>
    {
        private readonly ILogger<RestorePetHandler> _logger;
        private readonly IValidator<RestorePetCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;

        public RestorePetHandler(
            ILogger<RestorePetHandler> logger,
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

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogWarning("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            _logger.LogInformation("Pet {PetId} restored", petResult.Value.Id);

            var result = petResult.Value.Id.Value;

            return result;
        }
    }
}
