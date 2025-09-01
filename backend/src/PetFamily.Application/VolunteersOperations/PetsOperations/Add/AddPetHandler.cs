using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Application.SpeciesOperations;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.SpeciesManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.Add
{
    public class AddPetHandler
    {
        private readonly ILogger<AddPetHandler> _logger;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;

        public AddPetHandler(
            ILogger<AddPetHandler> logger,
            IValidator<AddPetCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteerResult = await _volunteersRepository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed to get volunteer {VolunteerId}", command.VolunteerId);

                return Errors.General.NotFound(command.VolunteerId).ToErrorList();
            }

            var petId = PetId.NewPetId();
            var name = command.Request.Name;
            var description = command.Request.Description;
            var speciesId = SpeciesId.Create(command.Request.SpeciesAndBreed.SpeciesId);
            var breedId = BreedId.Create(command.Request.SpeciesAndBreed.BreedId);

            var ExistsBreendInSpeciesResult = await _speciesRepository
                .ExistsBreedInSpecies(speciesId, breedId);
            if (ExistsBreendInSpeciesResult.IsFailure)
            {
                _logger.LogWarning("Breed {BreedId} not found in species {SpeciesId}", breedId, speciesId);

                return ExistsBreendInSpeciesResult.Error.ToErrorList();
            }

            var speciesAndBreed = SpeciesAndBreed.Create(speciesId, breedId).Value;
            var color = command.Request.Color;
            var healthInformation = command.Request.HealthInformation;
            var address = Address.Create(
                command.Request.Address.Street,
                command.Request.Address.HouseNumber,
                command.Request.Address.City,
                command.Request.Address.Country).Value;
            var weightKg = command.Request.WeightKg;
            var heightCm = command.Request.HeightCm;
            var numberPhone = PhoneNumber.Create(command.Request.NumberPhone).Value;
            var isCastrated = command.Request.isCastrated;
            var isVacinated = command.Request.isVaccinated;
            var birthDate = command.Request.BirthDate;
            var supportStatus = Enum.Parse<SupportStatus>(command.Request.SupportStatus);

            var petToAddResult = Pet.Create(
                petId,
                name,
                description,
                speciesAndBreed,
                color,
                healthInformation,
                address,
                weightKg,
                heightCm,
                numberPhone,
                isCastrated,
                isVacinated,
                birthDate,
                supportStatus);

            if (petToAddResult.IsFailure)
            {
                _logger.LogWarning(
                    "Pet {PetId} creation failed: {Errors}",
                    petToAddResult.Value.Id,
                    petToAddResult.Error);

                return Errors.General.ValueIsInvalid("pet").ToErrorList();
            }

            var petResult = volunteerResult.Value.AddPet(petToAddResult.Value);
            if (petResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to add pet {PetId}: {Errors}",
                    petResult.Value.Id,
                    petResult.Error);

                return petResult.Error.ToErrorList();
            }

            var donationsInfo = command.Request.DonationsInfo?
                .Select(di => DonationInfo.Create(di.Title, di.Description).Value) ?? [];

            var errorsAddDonationsInfo = volunteerResult.Value.AddDonationsInfoToPet(petId, donationsInfo);
            if (errorsAddDonationsInfo.Any())
            {
                _logger.LogWarning(
                    "Failed to add donations info to pet: {Errors}", errorsAddDonationsInfo);

                return errorsAddDonationsInfo;
            }

            await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            var result = petResult.Value.Id.Value;

            _logger.LogInformation("Pet {PetId} added", result);

            return result;
        }
    }
}