using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(Guid VolunteerId, UpdateSocialNetworksRequest Request);
}
