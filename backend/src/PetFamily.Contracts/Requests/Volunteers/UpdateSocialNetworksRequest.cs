using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Contracts.Requests.Volunteers
{
    public record UpdateSocialNetworksRequest(
        IEnumerable<SocialNetworkDto> SocialNetworks);
}
