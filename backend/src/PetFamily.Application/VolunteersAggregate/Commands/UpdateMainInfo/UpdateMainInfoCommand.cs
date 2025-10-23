using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolunteerId, UpdateMainInfoRequest Request) : ICommand;
}
