using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Contracts.Requests.Volunteers
{
    public record UpdateMainInfoRequest(
        FullNameDto FullName, 
        string Email, 
        string PhoneNumber, 
        string Description, 
        int ExperienceYears);
}