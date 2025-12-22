using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolunteerId, UpdateMainInfoRequest Request) : ICommand;
}
