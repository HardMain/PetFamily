using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Move
{
    public record MovePetCommand(
        Guid VolunteerId, Guid PetId, MovePetRequest Request) : ICommand;
}
