using Volunteers.Contracts.DTOs;

namespace Volunteers.Contracts.Requests
{
    public record UpdateMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string PhoneNumber,
        string Description,
        int ExperienceYears);
}