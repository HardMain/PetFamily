using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Aggregates.Species.Entities;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.SpeciesOperations.BreedsOperations.Add
{
    public class AddBreedHandler
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IValidator<AddBreedCommand> _validator;
        private readonly ILogger<AddBreedHandler> _logger;

        public AddBreedHandler(
            ISpeciesRepository speciesRepository,
            IValidator<AddBreedCommand> validator,
            ILogger<AddBreedHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddBreedCommand command, CancellationToken cancellationToken = default)
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
                _logger.LogWarning("Failed to get species {SpeciesId}: {Errors}", speciesId, speciesResult.Error);

                return speciesResult.Error.ToErrorList();
            }

            var name = command.Request.Name;
            var breedId = BreedId.NewBreedId();
            var breed = Breed.Create(breedId, name).Value;

            speciesResult.Value.AddBreed(breed);

            var result = await _speciesRepository.Save(speciesResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogWarning("Breed {BreedId} added", result.Value);

            return result.Value;
        }
    }
}
