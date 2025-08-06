using PetFamily.Contracts.DTOs.Shared;

namespace PetFamily.Contracts.Requests.Volunteers
{
    public record UpdateDonationsInfoRequest(
        IEnumerable<DonationInfoDTO> DonationsInfo);
}
