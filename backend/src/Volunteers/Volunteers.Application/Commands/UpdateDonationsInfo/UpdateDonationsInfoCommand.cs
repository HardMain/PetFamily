using Core.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(
        Guid VolunteerId, UpdateDonationsInfoRequest Request) : ICommand;
}
