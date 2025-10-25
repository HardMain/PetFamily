using PetFamily.Contracts.Shared;

namespace PetFamily.Contracts.VolunteersAggregate.Requests
{
    public record UpdateDonationsInfoRequest(
        IEnumerable<DonationInfoDto> DonationsInfo);
}
