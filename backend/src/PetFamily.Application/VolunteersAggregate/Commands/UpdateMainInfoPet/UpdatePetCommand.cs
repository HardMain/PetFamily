using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfoPet
{
    public record UpdatePetCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetRequest Request) : ICommand;
}
