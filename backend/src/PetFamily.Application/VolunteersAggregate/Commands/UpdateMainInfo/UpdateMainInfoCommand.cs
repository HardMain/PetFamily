using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolunteerId, UpdateMainInfoRequest Request) : ICommand;
}
