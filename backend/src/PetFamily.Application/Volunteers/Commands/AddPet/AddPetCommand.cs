using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.Volunteers.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request) : ICommand;
}