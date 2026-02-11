using Core.Abstractions;
using Core.Extensions;
using Core.Providers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;

namespace Volunteers.Application.Commands.Delete
{
    public class HardDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
    {
        const string BUCKET_NAME = "files";

        private readonly ILogger<HardDeleteVolunteerHandler> _logger;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly IFilesProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;

        public HardDeleteVolunteerHandler(
            ILogger<HardDeleteVolunteerHandler> logger,
            IValidator<DeleteVolunteerCommand> validator,
            IFilesProvider fileProvider,
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
                .Select(obj => new FileStorageDeleteDto(obj.PathToStorage.Path, BUCKET_NAME));

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