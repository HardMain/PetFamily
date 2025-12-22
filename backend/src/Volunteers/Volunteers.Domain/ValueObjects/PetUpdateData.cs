using SharedKernel.ValueObjects;

namespace Volunteers.Domain.ValueObjects
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
