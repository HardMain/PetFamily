using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.Move
{
    public record MovePetCommand(
        Guid VolunteerId, Guid PetId, MovePetRequest Request) : ICommand;
}
