using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update
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