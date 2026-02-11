using Core.Abstractions;
using Core.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Species.Contracts;
using Volunteers.Application.Abstractions;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.UpdateMainInfoPet
{
    public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
    {
        private readonly IValidator<UpdatePetCommand> _validator;
        private readonly ILogger<UpdatePetHandler> _logger;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesContract _speciesContract;

        public UpdatePetHandler(
            IValidator<UpdatePetCommand> validator,
            ILogger<UpdatePetHandler> logger,
            IVolunteersRepository volunteersRepository,
            IVolunteersReadDbContext readDbContext,
            ISpeciesContract speciesContract)
        {
            _validator = validator;
            _logger = logger;
            _volunteersRepository = volunteersRepository;
            _speciesContract = speciesContract;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var name = command.Request.Name;
            var description = command.Request.Description;
            var speciesId = SpeciesId.Create(command.Request.SpeciesAndBreed.SpeciesId);
            var breedId = BreedId.Create(command.Request.SpeciesAndBreed.BreedId);

            var isExistsBreedInSpecies = await _speciesContract
                .BreedExistsInSpeciesAsync(breedId.Value, speciesId.Value);

            if (!isExistsBreedInSpecies)
            {
                _logger.LogWarning("Breed {BreedId} not found in species {SpeciesId}", breedId, speciesId);

                return Errors.SpeciesAndBreed.NotFound(speciesId, breedId).ToErrorList();
            }

            var speciesAndBreed = SpeciesAndBreed.Create(speciesId, breedId);
            var color = command.Request.Color;
            var healthInformation = command.Request.HealthInformation;
            var address = Address.Create(
                command.Request.Address.Street,
                command.Request.Address.HouseNumber,
                command.Request.Address.City,
                command.Request.Address.Country);
            var weightKg = command.Request.WeightKg;
            var heightCm = command.Request.HeightCm;
            var ownerPhone = PhoneNumber.Create(command.Request.OwnerPhone);
            var isCastrated = command.Request.isCastrated;
            var birthDate = command.Request.BirthDate;
            var isVaccinated = command.Request.isVaccinated;
            var donationsInfo = ListDonationInfo.Create(command.Request.DonationsInfo
                .Select(di => DonationInfo.Create(di.Title, di.Description).Value));

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get volunteer {VolunteerId}", volunteerId);

                return volunteerResult.Error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to get pet {PetId}", petId);

                return petResult.Error.ToErrorList();
            }

            var petUpdateData = new PetUpdateData(
                name,
                description,
                speciesAndBreed.Value,
                color,
                healthInformation,
                address.Value,
                weightKg,
                heightCm,
                ownerPhone.Value,
                isCastrated,
                birthDate,
                isVaccinated,
                donationsInfo.Value);

            volunteerResult.Value.UpdatePet(petResult.Value, petUpdateData);

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogWarning("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            _logger.LogInformation("Pet updated {PetId}", petId);

            return petId.Value;
        }
    }
}
