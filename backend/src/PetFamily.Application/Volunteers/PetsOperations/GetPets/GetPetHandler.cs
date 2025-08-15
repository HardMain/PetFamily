using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.PetsOperations.AddPets;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.PetsOperations.GetPets
{
    public class GetPetHandler
    {
        private readonly ILogger<GetPetHandler> _logger;
        //private readonly IValidator<AddPetCommand> _validator;
        private readonly IFileProvider _fileProvider;

        public GetPetHandler(
            ILogger<GetPetHandler> logger,
            //IValidator<AddPetCommand> validator,
            IFileProvider fileProvider)
        {
            _logger = logger;
            //_validator = validator;
            _fileProvider = fileProvider;
        }

        public async Task<Result<string>> Handle(
            GetPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _fileProvider.GetFile(command.FileMetadataDTO, cancellationToken);
            if (result.IsFailure)
                return result.Error;

            _logger.LogInformation("File uploaded with id {fileId}", result.Value);

            return result;
        }
    }
}
