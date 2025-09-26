using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update
{
    public record UpdatePetCommand(
        Guid VolunteerId, 
        Guid PetId, 
        UpdatePetRequest Request) : ICommand;
}
