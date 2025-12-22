using Volunteers.Contracts.DTOs;

namespace Volunteers.Contracts.Requests
{
    public record UpdateSocialNetworksRequest(
        IEnumerable<SocialNetworkDto> SocialNetworks);
}
