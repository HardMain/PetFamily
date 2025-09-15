using PetFamily.Contracts.DTOs.Shared;

namespace PetFamily.Contracts.DTOs.Volunteers
{
    public class VolunteerReadDto
    {
        public Guid Id { get; init; }
        public FullNameDto FullName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Description { get; init; } = default!;
        public int ExperienceYears { get; init; }
        public string PhoneNumber { get; init; } = default!;
        public List<SocialNetworkDto> SocialNetworks { get; init; } = [];
        public List<DonationInfoDto> DonationsInfo { get; init; } = [];
    }
}