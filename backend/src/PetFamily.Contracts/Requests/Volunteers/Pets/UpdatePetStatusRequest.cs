using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Contracts.Requests.Volunteers.Pets
{
    public record UpdatePetStatusRequest(PetSupportStatusDto SupportStatus);
}