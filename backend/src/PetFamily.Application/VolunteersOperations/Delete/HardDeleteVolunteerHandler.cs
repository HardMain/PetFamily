using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersOperations.Delete
{
    public class HardDeleteVolunteerHandler
    {
        const string BUCKET_NAME = "files";

        private readonly ILogger<HardDeleteVolunteerHandler> _logger;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public HardDeleteVolunteerHandler(
            ILogger<HardDeleteVolunteerHandler> logger,
            IValidator<DeleteVolunteerCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository repository)
        {
            _logger = logger;
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteVolunteerCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var AllPetsFiles = volunteerResult.Value.GetAllPetsFiles();

            var filesStorageDelete = AllPetsFiles
                .Select(obj => new FileStorageDeleteDTO(obj.PathToStorage.Path, BUCKET_NAME));

            var deleteFilesFromMinioResult = await _fileProvider.DeleteFiles(filesStorageDelete, cancellationToken);
            if (deleteFilesFromMinioResult.IsFailure)
            {
                _logger.LogWarning("Failed to delete pet files from MinIO: {Errors}",
                    deleteFilesFromMinioResult.Error);

                return deleteFilesFromMinioResult.Error;
            }

            var result = await _volunteersRepository.Delete(volunteerResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Volunteer {volunteerId} deleted(hard)", volunteerId);

            return result.Value;
        }
    }
}