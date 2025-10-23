using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.MovePet
{
    public record MovePetCommand(
        Guid VolunteerId, Guid PetId, MovePetRequest Request) : ICommand;
}
