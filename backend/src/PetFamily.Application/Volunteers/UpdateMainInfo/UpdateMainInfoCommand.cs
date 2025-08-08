using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoCommand(Guid VolunteerId, UpdateMainInfoRequest Request);
}
