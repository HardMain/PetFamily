using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.PetsOperations.AddPets
{
    public class AddPetHandler
    {
        private readonly ILogger<AddPetHandler> _logger;
        //private readonly IValidator<AddPetCommand> _validator;
        private readonly IFileProvider _fileProvider;

        public AddPetHandler(
            ILogger<AddPetHandler> logger,
            //IValidator<AddPetCommand> validator,
            IFileProvider fileProvider)
        {
            _logger = logger;
            //_validator = validator;
            _fileProvider = fileProvider;
        }

        public async Task<Result<string>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider.UploadFile(command.FileMetadataDTO, cancellationToken);
            if (result.IsFailure)
                return result.Error;

            _logger.LogInformation("File uploaded with id {fileId}", result.Value);

            return result;
        }
    }
}