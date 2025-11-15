using Volunteers.Contracts.DTOs;

namespace Volunteers.Contracts.Requests
{
    public record UpdatePetSupportStatusRequest(PetSupportStatusDto SupportStatus);
}