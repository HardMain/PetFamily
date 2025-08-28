using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.DeletePetFiles
{
    public class DeletePetFilesHandler
    {
        const string BUCKET_NAME = "files";

        private readonly ILogger<DeletePetFilesHandler> _logger;
        private readonly IValidator<DeletePetFilesCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public DeletePetFilesHandler(
            ILogger<DeletePetFilesHandler> logger,
            IValidator<DeletePetFilesCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository)
        {
            _logger = logger;
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<IReadOnlyList<string>, ErrorList>> Handle(
            DeletePetFilesCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository
                .GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer {VolunteerId}: {Errors}",
                    volunteerId,
                    volunteerResult.Error);

                return volunteerResult.Error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning("Failed to get pet {PetId}: {Errors}",
                     petId,
                     petResult.Error);

                return petResult.Error.ToErrorList();
            }

            var filePathsResult = command.Request.ObjectNameList.Select(FilePath.Create);
            if (filePathsResult.Any(p => p.IsFailure))
                return Errors.General.ValueIsInvalid("filePath").ToErrorList();

            var petFiles = filePathsResult.Select(f => new PetFile(f.Value));

            var deleteFilesResult = petResult.Value.DeleteFiles(petFiles);
            if (deleteFilesResult.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files: {Errors}",
                     deleteFilesResult.Error);

                return deleteFilesResult.Error.ToErrorList();
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var filesStorageDelete = command.Request.ObjectNameList
                .Select(obj => new FileStorageDeleteDTO(obj, BUCKET_NAME));

            var result = await _fileProvider.DeleteFiles(filesStorageDelete, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files from MinIO: {Errors}",
                    result.Error);

                return result.Error;
            }

            _logger.LogInformation("Files deleted: {deletedFiles}", result.Value);

            return result;
        }
    }
}