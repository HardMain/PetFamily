using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersOperations.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(Guid VolunteerId, UpdateSocialNetworksRequest Request);
}
