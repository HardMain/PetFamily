using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfoPet
{
    public record UpdatePetCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetRequest Request) : ICommand;
}
