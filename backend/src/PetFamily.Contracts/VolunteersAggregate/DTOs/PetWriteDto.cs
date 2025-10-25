using PetFamily.Contracts.Shared;
using PetFamily.Contracts.SpeciesAggregate.DTOs;

namespace PetFamily.Contracts.VolunteersAggregate.DTOs
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