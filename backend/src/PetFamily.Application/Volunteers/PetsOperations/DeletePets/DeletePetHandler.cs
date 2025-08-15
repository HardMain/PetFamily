using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.PetsOperations.DeletePets
{
    public class DeletePetHandler
    {
        //private readonly IValidator<DeletePetHandler> _validator;
        private readonly ILogger<DeletePetCommand> _logger;
        private readonly IFileProvider _fileProvider;

        public DeletePetHandler(
            //IValidator<DeletePetHandler> validator,
            ILogger<DeletePetCommand> logger,
            IFileProvider fileProvider)
        {
            //_validator = validator;
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public async Task<Result<string>> Handle(
            DeletePetCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider.DeleteFile(command.FileMetadataDTO, cancellationToken);
            if (result.IsFailure)
                return result.Error;

            _logger.LogInformation("File uploaded with id {fileId}", result.Value);

            return result;
        }
    }
}
