using PetFamily.Contracts.Shared;
using PetFamily.Contracts.SpeciesAggregate.DTOs;
using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Contracts.VolunteersAggregate.Requests
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
        string OwnerPhone,
        bool isCastrated,
        DateTime BirthDate,
        bool isVaccinated,
        PetSupportStatusDto SupportStatus,
        IEnumerable<DonationInfoDto>? DonationsInfo);
}