using Core.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        AddPetRequest Request) : ICommand;
}