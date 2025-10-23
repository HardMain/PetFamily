using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request) : ICommand;
}