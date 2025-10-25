using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(
        Guid VolunteerId, UpdateDonationsInfoRequest Request) : ICommand;
}
