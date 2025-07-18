using PetFamily.Contracts.DTOs.Pets;
using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(
        FullNameDTO name, 
        string Email, 
        string Description, 
        int ExperienceYears,
        string PhoneNumber,
        IEnumerable<SocialNetworksDTO>? SocialNetworks,
        IEnumerable<DonationsInfoDTO>? DonationsInfo
    );
}