using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update;
using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Volunteers.IntegrationTests.Helpers.Mappers
{
    public static class PetMapper
    {
        public static UpdatePetRequest ToUpdateRequest(PetReadDto pet)
        {
            return new UpdatePetRequest(
                pet.Name,
                pet.Description,
                new SpeciesAndBreedDto(
                    pet.SpeciesAndBreed.SpeciesId, pet.SpeciesAndBreed.BreedId),
                pet.Color,
                pet.HealthInformation,
                new AddressDto(
                    pet.Address.Street, pet.Address.HouseNumber, pet.Address.City, pet.Address.Country),
                pet.WeightKg,
                pet.HeightCm,
                pet.OwnerPhone,
                pet.isCastrated,
                pet.BirthDate,
                pet.isVaccinated,
                pet.DonationsInfo.Select(d => new DonationInfoDto(d.Title, d.Description))
                );
        }
    }
}
