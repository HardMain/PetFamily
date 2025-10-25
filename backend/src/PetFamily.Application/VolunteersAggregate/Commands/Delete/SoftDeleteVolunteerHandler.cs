using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteersAggregate;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersAggregate.Commands.Delete
{
    public class SoftDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

        public SoftDeleteVolunteerHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<DeleteVolunteerCommand> validator,
            ILogger<SoftDeleteVolunteerHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteVolunteerCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer with {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            volunteerResult.Value.SoftDelete(true);

            var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Deleted(soft) volunteer with id {volunteerId}", volunteerId);

            return result.Value;
        }
    }
}
