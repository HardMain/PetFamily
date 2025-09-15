using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.Add
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request) : ICommand;
}