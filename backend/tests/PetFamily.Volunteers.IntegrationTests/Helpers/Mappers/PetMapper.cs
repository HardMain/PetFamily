using PetFamily.Contracts.Shared;
using PetFamily.Contracts.SpeciesAggregate.DTOs;
using PetFamily.Contracts.VolunteersAggregate.DTOs;
using PetFamily.Contracts.VolunteersAggregate.Requests;

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
