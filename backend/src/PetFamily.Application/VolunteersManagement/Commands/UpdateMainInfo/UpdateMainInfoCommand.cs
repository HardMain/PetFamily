using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersManagement.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolunteerId, UpdateMainInfoRequest Request) : ICommand;
}
