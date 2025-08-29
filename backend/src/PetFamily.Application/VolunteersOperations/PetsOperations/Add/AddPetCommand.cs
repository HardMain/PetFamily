using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.Add
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request);
}