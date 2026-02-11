using Core.Abstractions;
using Core.Extensions;
using Core.Providers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.DeletePetFiles
{
    public class DeletePetFilesHandler : ICommandHandler<IReadOnlyList<string>, DeletePetFilesCommand>
    {
        const string BUCKET_NAME = "files";

        private readonly ILogger<DeletePetFilesHandler> _logger;
        private readonly IValidator<DeletePetFilesCommand> _validator;
        private readonly IFilesProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public DeletePetFilesHandler(
            ILogger<DeletePetFilesHandler> logger,
            IValidator<DeletePetFilesCommand> validator,
            IFilesProvider fileProvider,
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

            var deleteFilesResult = volunteerResult.Value.DeleteFilesFromPet(petId, petFiles);
            if (deleteFilesResult.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files: {Errors}",
                     deleteFilesResult.Error);

                return deleteFilesResult.Error.ToErrorList();
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogWarning("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var filesStorageDelete = command.Request.ObjectNameList
                .Select(obj => new FileStorageDeleteDto(obj, BUCKET_NAME));

            var result = await _fileProvider.DeleteFiles(filesStorageDelete, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files from MinIO: {Errors}",
                    result.Error);

                return result.Error;
            }

            _logger.LogInformation("Files deleted: {DeletedFiles}", result.Value);

            return result;
        }
    }
}