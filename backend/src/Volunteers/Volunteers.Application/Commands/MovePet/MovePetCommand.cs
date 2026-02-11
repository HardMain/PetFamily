using Core.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.MovePet
{
    public record MovePetCommand(
        Guid VolunteerId, Guid PetId, MovePetRequest Request) : ICommand;
}
