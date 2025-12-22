using Core.Dtos;

namespace Volunteers.Contracts.Requests
{
    public record UpdateDonationsInfoRequest(
        IEnumerable<DonationInfoDto> DonationsInfo);
}
