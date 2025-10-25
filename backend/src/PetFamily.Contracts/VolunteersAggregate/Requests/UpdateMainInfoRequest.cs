using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Contracts.VolunteersAggregate.Requests
{
    public record UpdateMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string PhoneNumber,
        string Description,
        int ExperienceYears);
}