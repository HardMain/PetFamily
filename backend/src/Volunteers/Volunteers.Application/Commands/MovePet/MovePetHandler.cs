using Core.Abstractions;
using Core.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.MovePet
{
    public class MovePetHandler : ICommandHandler<Guid, MovePetCommand>
    {
        private readonly IValidator<MovePetCommand> _validator;
        private readonly ILogger<MovePetHandler> _logger;
        private readonly IVolunteersRepository _volunteersRepository;

        public MovePetHandler(
            IValidator<MovePetCommand> validator,
            ILogger<MovePetHandler> logger,
            IVolunteersRepository volunteersRepository)
        {
            _validator = validator;
            _logger = logger;
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            MovePetCommand command, CancellationToken cancellationToken = default)
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
                _logger.LogWarning(
                    "Failed to get volunteer {VolunteerId}: {Errors}",
                    volunteerId,
                    volunteerResult.Error);

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

            var positionResult = Position.Create(command.Request.newPosition);
            if (positionResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to create position for pet {PetId}: {Errors}",
                    petId,
                    positionResult.Error);

                return positionResult.Error.ToErrorList();
            }

            var moveResult = volunteerResult.Value.MovePet(petResult.Value, positionResult.Value);
            if (moveResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to move pet {PetId}: {Errors}",
                    petId,
                    moveResult.Error);
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            _logger.LogInformation("Pet {PetId} moved", saveResult);

            var result = moveResult.Value.Id.Value;

            return result;
        }
    }
}
