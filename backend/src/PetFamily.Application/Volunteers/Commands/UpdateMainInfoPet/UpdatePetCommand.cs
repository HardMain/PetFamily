using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet
{
    public record UpdatePetCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetRequest Request) : ICommand;
}
