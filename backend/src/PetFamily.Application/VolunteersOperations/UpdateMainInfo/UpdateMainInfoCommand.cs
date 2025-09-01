using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersOperations.UpdateMainInfo
{
    public record UpdateMainInfoCommand(Guid VolunteerId, UpdateMainInfoRequest Request);
}
