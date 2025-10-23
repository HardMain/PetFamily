using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdateDonationsInfo
{
    public class UpdateDonationsInfoHandler : ICommandHandler<Guid, UpdateDonationsInfoCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<UpdateDonationsInfoCommand> _validator;
        private readonly ILogger<UpdateDonationsInfoHandler> _logger;

        public UpdateDonationsInfoHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<UpdateDonationsInfoCommand> validator,
            ILogger<UpdateDonationsInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateDonationsInfoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer with {volunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var donationsInfo = ListDonationInfo.Create(command.Request.DonationsInfo
                .Select(di => DonationInfo.Create(di.Title, di.Description).Value));

            volunteerResult.Value.SetListDonationInfo(donationsInfo.Value);

            var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", result.Error);

                return result.Error.ToErrorList();
            }

            _logger.LogInformation("Donations info updated for volunteer {volunteerId}", volunteerId);

            return result.Value;
        }
    }
}