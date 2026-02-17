using Core.Abstractions;
using Core.Extensions;
using Core.Providers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;

namespace Volunteers.Application.Commands.DeletePet
{
    public class HardDeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
    {
        const string BUCKET_NAME = "files";

        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<HardDeletePetHandler> _logger;
        private readonly IFilesProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public HardDeletePetHandler(
            IValidator<DeletePetCommand> validator,
            ILogger<HardDeletePetHandler> logger,
            IFilesProvider fileProvider,
            IVolunteersRepository volunteersRepository)
        {
            _validator = validator;
            _logger = logger;
            _fileProvider = fileProvider;
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

            var petFilesResult = volunteerResult.Value.GetPetFiles(petId);
            if (petFilesResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get pet files {PetId}: {Errors}",
                    petId,
                    petFilesResult.Error);

                return petFilesResult.Error.ToErrorList();
            }

            var filesStorageDelete = petFilesResult.Value
                .Select(obj => new FileStorageDeleteDto(obj.PathToStorage.Path, BUCKET_NAME));

            var deleteFilesFromMinioResult = await _fileProvider.DeleteFiles(filesStorageDelete, cancellationToken);
            if (deleteFilesFromMinioResult.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files from MinIO: {Errors}",
                    deleteFilesFromMinioResult.Error);

                return deleteFilesFromMinioResult.Error;
            }

            var deletedPetResult = volunteerResult.Value.HardDeletePet(petResult.Value);
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
                _logger.LogWarning("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = deletedPetResult.Value.Id.Value;

            _logger.LogInformation("Pet {PetId} deleted", result);

            return result;
        }
    }
}