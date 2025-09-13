using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Contracts.Requests.Volunteers
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