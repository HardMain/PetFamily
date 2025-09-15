using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;

namespace PetFamily.Contracts.Requests.Volunteers.Pets
{
    public record AddPetRequest(
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
        string SupportStatus,
        IEnumerable<DonationInfoDto>? DonationsInfo);
}