using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Contracts.Requests.Volunteers.Pets
{
    public record UpdatePetSupportStatusRequest(PetSupportStatusDto SupportStatus);
}