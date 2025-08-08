using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(Guid VolunteerId, UpdateDonationsInfoRequest Request);
}
