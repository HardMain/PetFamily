using Core.Dtos;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Contracts.Requests
{
    public record UpdatePetRequest(
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
        IEnumerable<DonationInfoDto> DonationsInfo);
}