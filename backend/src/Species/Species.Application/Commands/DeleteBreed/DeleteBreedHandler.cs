using Core.Abstractions;
using Core.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Species.Application.Abstractions;
using Volunteers.Contracts;

namespace Species.Application.Commands.DeleteBreed
{
    public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
    {
        private readonly IValidator<DeleteBreedCommand> _validator;
        private readonly ILogger<DeleteBreedHandler> _logger;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolunteersContract _volunteersContract;

        public DeleteBreedHandler(
            IValidator<DeleteBreedCommand> validator,
            ILogger<DeleteBreedHandler> logger,
            ISpeciesRepository speciesRepository,
            ISpeciesReadDbContext speciesReadDbContext,
            IVolunteersContract volunteersContract)
        {
            _validator = validator;
            _logger = logger;
            _speciesRepository = speciesRepository;
            _volunteersContract = volunteersContract;
        }

        public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());
                return validationResult.ToErrorList();
            }

            var speciesId = SpeciesId.Create(command.SpeciesId);

            var speciesResult = await _speciesRepository.GetById(speciesId, cancellationToken);
            if (speciesResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get species {SpeciesId}: {Errors}",
                    speciesId,
                    speciesResult.Error);

                return speciesResult.Error.ToErrorList();
            }

            var breedId = BreedId.Create(command.BreedId);

            var breedResult = speciesResult.Value.GetBreedById(breedId);
            if (breedResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get breed {BreedId}: {Errors}",
                    breedId,
                    breedResult.Error);

                return breedResult.Error.ToErrorList();
            }

            var isBreedInUse = await _volunteersContract.IsBreedInUseAsync(breedId);
            if (isBreedInUse)
            {
                _logger.LogWarning(
                    "Failed to delete breed {BreedId}: it is in use", breedId);

                return Errors.SpeciesAndBreed.BreedInUse(breedId).ToErrorList();
            }

            var deletedBreedResult = speciesResult.Value.DeleteBreed(breedResult.Value);

            var saveResult = await _speciesRepository.Save(speciesResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogWarning("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = breedResult.Value.Id.Value;

            _logger.LogInformation("Breed {BreedId} deleted", result);

            return result;
        }
    }
}
