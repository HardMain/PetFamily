using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Contracts.DTOs.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerCommand(
        FullNameDTO FullName,
        string Email, 
        string Description, 
        int ExperienceYears, 
        string PhoneNumber,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationInfoDTO> DonationsInfo
    );
}