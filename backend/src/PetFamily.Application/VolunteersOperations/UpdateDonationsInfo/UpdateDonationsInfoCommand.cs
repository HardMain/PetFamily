using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersOperations.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(Guid VolunteerId, UpdateDonationsInfoRequest Request);
}
