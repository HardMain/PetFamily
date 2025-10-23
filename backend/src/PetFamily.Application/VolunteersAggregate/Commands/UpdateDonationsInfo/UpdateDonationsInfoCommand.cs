using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(
        Guid VolunteerId, UpdateDonationsInfoRequest Request) : ICommand;
}
