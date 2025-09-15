using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;

namespace PetFamily.Contracts.DTOs.Volunteers.Pets
{
    public record PetWriteDto(
        string Name,
        string Description,
        SpeciesAndBreedDto SpeciesAndBreed,
        string Color,
        string HealthInformation,
        AddressDto Address,
        double WeightKg,
        double HeightCm,
        string NumberPhone,
        bool isCastrated,
        DateTime BirthDate,
        bool isVaccinated,
        string SupportStatus);
}