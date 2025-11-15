using Core.Dtos;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Contracts.Requests
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