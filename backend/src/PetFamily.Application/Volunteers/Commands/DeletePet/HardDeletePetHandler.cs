using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Volunteers.Commands.DeletePet
{
    public class HardDeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
    {
        const string BUCKET_NAME = "files";

        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<HardDeletePetHandler> _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public HardDeletePetHandler(
            IValidator<DeletePetCommand> validator,
            ILogger<HardDeletePetHandler> logger,
            IFileProvider fileProvider,
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
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = deletedPetResult.Value.Id.Value;

            _logger.LogInformation("Pet {PetId} deleted", result);

            return result;
        }
    }
}