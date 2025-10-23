using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesAggregate;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed
{
    public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
    {
        private readonly IValidator<DeleteBreedCommand> _validator;
        private readonly ILogger<DeleteBreedHandler> _logger;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;

        public DeleteBreedHandler(
            IValidator<DeleteBreedCommand> validator,
            ILogger<DeleteBreedHandler> logger,
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext)
        {
            _validator = validator;
            _logger = logger;
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
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
                    "Failed to get volunteer {VolunteerId}: {Errors}",
                    speciesId,
                    speciesResult.Error);

                return speciesResult.Error.ToErrorList();
            }

            var breedId = BreedId.Create(command.BreedId);

            var breedResult = speciesResult.Value.GetBreedById(breedId);
            if (breedResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get pet {PetId}: {Errors}",
                    breedResult,
                    breedResult.Error);

                return breedResult.Error.ToErrorList();
            }

            var breedInUse = await _readDbContext.Pets.AnyAsync(p => p.SpeciesAndBreed.BreedId == breedId);
            if (breedInUse)
            {
                _logger.LogWarning(
                    "Failed to delete breed {BreedId}: it is in use", breedId);

                return Errors.SpeciesAndBreed.BreedInUse(breedId).ToErrorList();
            }

            var deletedBreedResult = speciesResult.Value.DeleteBreed(breedResult.Value);

            var saveResult = await _speciesRepository.Save(speciesResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = breedResult.Value.Id.Value;

            _logger.LogInformation("Breed {BreedId} to deleted", result);

            return result;
        }
    }
}
