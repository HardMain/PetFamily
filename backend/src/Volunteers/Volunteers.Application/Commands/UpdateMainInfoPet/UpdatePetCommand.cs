using Core.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.UpdateMainInfoPet
{
    public record UpdatePetCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetRequest Request) : ICommand;
}
