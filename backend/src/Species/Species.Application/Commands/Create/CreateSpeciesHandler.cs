using Core.Abstractions;
using Core.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Species.Application.Abstractions;
using SpeciesEntity = Species.Domain.Entities.Species;

namespace Species.Application.Commands.Create
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

            var species = SpeciesEntity.Create(speciesId, name).Value;

            var result = await _speciesRepository.Add(species, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogWarning("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Species {SpeciesId} added", speciesId);

            return result.Value;
        }
    }
}
