using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.SpeciesManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Add
{
    public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private readonly ILogger<AddPetHandler> _logger;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IReadDbContext _readDbContext;

        public AddPetHandler(
            ILogger<AddPetHandler> logger,
            IValidator<AddPetCommand> validator,
            IVolunteersRepository volunteersRepository,
            IReadDbContext readDbContext)
        {
            _logger = logger;
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _readDbContext = readDbContext;
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

            var isExistsBreedInSpecies = await _readDbContext.Breeds
                .AnyAsync(b => b.Id == breedId.Value && b.SpeciesId == speciesId.Value);
            if (!isExistsBreedInSpecies)
            {
                _logger.LogWarning("Breed {BreedId} not found in species {SpeciesId}", breedId, speciesId);

                return Errors.SpeciesAndBreed.NotFound(speciesId, breedId).ToErrorList();
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
            var numberPhone = PhoneNumber.Create(command.Request.OwnerPhone).Value;
            var isCastrated = command.Request.isCastrated;
            var isVacinated = command.Request.isVaccinated;
            var birthDate = command.Request.BirthDate;
            
            var petSupportStatus = command.Request.SupportStatus;
            if (!Enum.IsDefined(typeof(PetSupportStatusDto),  command.Request.SupportStatus))
            {
                _logger.LogWarning(
                    "Invalid support status {StatusValue} for pet {PetId}",
                    command.Request.SupportStatus,
                    petId);

                return Errors.General.ValueIsInvalid("supportStatus").ToErrorList();
            }

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
                (SupportStatus)petSupportStatus);

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

            if (command.Request.DonationsInfo is not null)
            {
                var donationsInfo = ListDonationInfo.Create(command.Request.DonationsInfo
                    .Select(di => DonationInfo.Create(di.Title, di.Description).Value));
                
                volunteerResult.Value.SetListDonationInfoToPet(petResult.Value, donationsInfo.Value);
            }

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
            {
                _logger.LogInformation("Failed to save data: {Errors}", saveResult.Error);

                return saveResult.Error.ToErrorList();
            }

            var result = petResult.Value.Id.Value;

            _logger.LogInformation("Pet {PetId} added", result);

            return result;
        }
    }
}