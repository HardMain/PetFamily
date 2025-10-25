using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Contracts.VolunteersAggregate.Requests
{
    public record UpdatePetSupportStatusRequest(PetSupportStatusDto SupportStatus);
}