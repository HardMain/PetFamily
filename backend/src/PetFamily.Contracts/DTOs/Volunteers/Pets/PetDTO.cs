using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;

namespace PetFamily.Contracts.DTOs.Volunteers.Pets
{
    public record PetDTO(
        string Name,
        string Description,
        SpeciesAndBreedDTO SpeciesAndBreed,
        string Color,
        string HealthInformation,
        AddressDTO Address,
        double WeightKg,
        double HeightCm,
        string NumberPhone,
        bool isCastrated,
        DateTime BirthDate,
        bool isVaccinated,
        string SupportStatus);
}