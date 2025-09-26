using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.SpeciesManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{
    public record PetUpdateData(
        string Name, 
        string Description,
        SpeciesAndBreed SpeciesAndBreed,
        string Color,
        string HealthInformation,
        Address Address,
        double WeightKg,
        double HeightCm,
        PhoneNumber OwnerPhone,
        bool isCastrated,
        DateTime BirthDate,
        bool isVaccinated,
        ListDonationInfo DonationsInfo);
}
