using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteersAggregate;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersAggregate.Commands.DeletePet
{
    public class SoftDeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
    {
        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<HardDeletePetHandler> _logger;
        private readonly IVolunteersRepository _volunteersRepository;

        public SoftDeletePetHandler(
            IValidator<DeletePetCommand> validator,
            ILogger<HardDeletePetHandler> logger,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository)
        {
            _validator = validator;
            _logger = logger;
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeletePetCommand command, CancellationToken cancellationToken = default)
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

            var deletedPetResult = volunteerResult.Value.SoftDeletePet(petResult.Value);
            if (deletedPetResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to delete pet {PetId}: {Errors}",
                    deletedPetResult.Value.Id,
                    deletedPetResult.Error);

                return deletedPetResult.Error.ToErrorList();
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = deletedPetResult.Value.Id.Value;

            _logger.LogInformation("Pet {PetId} to deleted", result);

            return result;
        }
    }
}