using PetFamily.Contracts.Shared;
using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Contracts.VolunteersAggregate.Requests
{
    public record CreateVolunteerRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int ExperienceYears,
        string PhoneNumber,
        IEnumerable<SocialNetworkDto>? SocialNetworks,
        IEnumerable<DonationInfoDto>? DonationsInfo
    );
}