using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request) : ICommand;
}