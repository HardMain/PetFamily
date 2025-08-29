using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.AddPetFiles
{
    public class AddPetFilesHandler
    {
        const string BUCKET_NAME = "files";

        private readonly ILogger<AddPetFilesHandler> _logger;
        private readonly IValidator<AddPetFilesCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public AddPetFilesHandler(
            ILogger<AddPetFilesHandler> logger,
            IValidator<AddPetFilesCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository)
        {
            _logger = logger;
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<IReadOnlyList<string>, ErrorList>> Handle(
            AddPetFilesCommand command,
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

                return Errors.General.NotFound(volunteerId).ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning("Failed to get pet {PetId}: {Errors}",
                    petId,
                    petResult.Error);

                return Errors.General.NotFound(petId).ToErrorList();
            }

            var pathListResult = GetPathList(command.Files);

            if (pathListResult.IsFailure)
            {
                _logger.LogWarning("Failed to get path list: {Errors}",
                    pathListResult.Error);

                return pathListResult.Error.ToErrorList();
            }

            var petFiles = pathListResult.Value.Select(path => new PetFile(path));

            var filesStorageUpload = pathListResult.Value.Zip(command.Files,
                (v1, v2) => new FileStorageUploadDTO(v2.Content, v1.Path, BUCKET_NAME));

            var result = await _fileProvider.UploadFiles(filesStorageUpload, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogWarning("Failed to upload files: {Errors}",
                    result.Error);

                return result.Error;
            }

            var addedFilesResult = volunteerResult.Value.AddFilesToPet(petId, petFiles);
            if (addedFilesResult.IsFailure)
            {
                _logger.LogWarning("Failed to add files to pet {PetId}: {Errors}",
                    petId,
                    addedFilesResult.Error);

                return addedFilesResult.Error.ToErrorList();
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                var filesStorageDelete = pathListResult.Value.Select(path => new FileStorageDeleteDTO(path.Path, BUCKET_NAME));

                var deleteResult = await _fileProvider.DeleteFiles(filesStorageDelete, cancellationToken);
                if (deleteResult.IsFailure)
                {
                    _logger.LogWarning("Failed to clean up MinIO files after failed database save: {Errors}",
                        deleteResult.Error);

                    return deleteResult.Error;
                }

                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            _logger.LogInformation("Files uploaded: {Files}", result.Value);

            return result;
        }
        private Result<List<FilePath>> GetPathList(IEnumerable<FileFormDTO> files)
        {
            List<FilePath> pathList = [];

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);

                var pathResult = FilePath.Create(Guid.NewGuid(), extension);
                if (pathResult.IsFailure)
                    return pathResult.Error;

                pathList.Add(pathResult.Value);
            }

            return pathList;
        }
    }
}