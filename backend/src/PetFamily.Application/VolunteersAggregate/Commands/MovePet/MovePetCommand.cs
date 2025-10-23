using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Commands.MovePet
{
    public record MovePetCommand(
        Guid VolunteerId, Guid PetId, MovePetRequest Request) : ICommand;
}
