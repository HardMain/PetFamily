using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesAggregate;
using PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.SpeciesAggregate.Commands.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
    {
        private readonly IValidator<DeleteSpeciesCommand> _validator;
        private readonly ILogger<DeleteBreedHandler> _logger;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;

        public DeleteSpeciesHandler(
            IValidator<DeleteSpeciesCommand> validator,
            ILogger<DeleteBreedHandler> logger,
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext)
        {
            _validator = validator;
            _logger = logger;
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
        }

        public async Task<Result<Guid, ErrorList>> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());
                return validationResult.ToErrorList();
            }

            var speciesId = SpeciesId.Create(command.Id);

            var speciesResult = await _speciesRepository.GetById(speciesId, cancellationToken);
            if (speciesResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get volunteer {VolunteerId}: {Errors}",
                    speciesId,
                    speciesResult.Error);

                return speciesResult.Error.ToErrorList();
            }

            var speciesInUse = await _readDbContext.Pets.AnyAsync(p => p.SpeciesAndBreed.SpeciesId == speciesId);
            if (speciesInUse)
            {
                _logger.LogWarning(
                    "Failed to delete species {SpeciesId}: it is in use", speciesId);

                return Errors.SpeciesAndBreed.SpeciesInUse(speciesId).ToErrorList();
            }

            var deletedSpeciesResult = await _speciesRepository.Delete(speciesResult.Value);
            if (deletedSpeciesResult.IsFailure)
            {
                _logger.LogInformation("Failed to delete species {SpeciesId}: {Errors}", speciesId, deletedSpeciesResult.Error);

                return deletedSpeciesResult.Error.ToErrorList();
            }

            var result = deletedSpeciesResult.Value;

            _logger.LogInformation("Species {SpeciesId} to deleted", result);

            return result;
        }
    }
}