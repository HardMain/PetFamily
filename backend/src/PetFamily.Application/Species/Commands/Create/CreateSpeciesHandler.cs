using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Species.Commands.Create
{
    public class CreateSpeciesHandler : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IValidator<CreateSpeciesCommand> _validator;
        private readonly ILogger<CreateSpeciesHandler> _logger;

        public CreateSpeciesHandler(
            ISpeciesRepository speciesRepository,
            IValidator<CreateSpeciesCommand> validator,
            ILogger<CreateSpeciesHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateSpeciesCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var name = command.Request.Name;
            var speciesId = SpeciesId.NewSpeciesId();

            var species = Species.Create(speciesId, name).Value;

            var result = await _speciesRepository.Add(species, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogWarning("Species {SpeciesId} added", speciesId);

            return result.Value;
        }
    }
}
