using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Contracts.Requests.Volunteers
{
    public record CreateVolunteerRequest(
        FullNameDTO FullName,
        string Email,
        string Description,
        int ExperienceYears,
        string PhoneNumber,
        IEnumerable<SocialNetworkDTO>? SocialNetworks,
        IEnumerable<DonationInfoDTO>? DonationsInfo
    );
}