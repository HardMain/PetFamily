using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.Move
{
    public record MovePetCommand(Guid VolunteerId, Guid PetId, MovePetRequest Request);
}
