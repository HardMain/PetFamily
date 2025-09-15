using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersManagement.Commands.UpdateDonationsInfo
{
    public record UpdateDonationsInfoCommand(
        Guid VolunteerId, UpdateDonationsInfoRequest Request) : ICommand;
}
