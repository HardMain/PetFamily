using Core.Abstractions;
using Core.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Species.Application.Abstractions;
using Species.Domain.Entities;

namespace Species.Application.Commands.AddBreed
{
    public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
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

            var result = speciesResult.Value.AddBreed(breed).Id.Value;

            var SaveResult = await _speciesRepository.Save(speciesResult.Value, cancellationToken);
            if (SaveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", SaveResult.Error);

                return SaveResult.Error.ToErrorList();
            }

            _logger.LogWarning("Breed {BreedId} added", result);

            return result;
        }
    }
}
