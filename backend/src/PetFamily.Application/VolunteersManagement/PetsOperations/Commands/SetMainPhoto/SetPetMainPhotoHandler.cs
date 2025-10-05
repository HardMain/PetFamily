using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.SetMainPhoto
{
    public class SetPetMainPhotoHandler : ICommandHandler<string, SetPetMainPhotoCommand>
    {
        private readonly ILogger<SetPetMainPhotoHandler> _logger;
        private readonly IValidator<SetPetMainPhotoCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;

        public SetPetMainPhotoHandler(
            ILogger<SetPetMainPhotoHandler> logger,
            IValidator<SetPetMainPhotoCommand> validator,
            IVolunteersRepository volunteersRepository)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = volunteersRepository;
        }
        public async Task<Result<string, ErrorList>> Handle(
            SetPetMainPhotoCommand command, 
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

            var mainPhoto = new PetFile(FilePath.Create(command.Request.Path).Value);

            var setMainPhotoResult = volunteerResult.Value.SetPetMainPhoto(petResult.Value, mainPhoto);
            if (setMainPhotoResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to set main photo to pet {PetId}: {Errors}",
                    petId,
                    setMainPhotoResult.Error);

                return setMainPhotoResult.Error.ToErrorList();
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            _logger.LogInformation("Pet {PetId} moved", saveResult);

            var result = setMainPhotoResult.Value.PathToStorage.Path;

            return result;
        }
    }
}
