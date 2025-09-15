using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersManagement.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolunteerId, UpdateSocialNetworksRequest Request) : ICommand;
}
