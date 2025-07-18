namespace PetFamily.Contracts.DTOs.Volunteers
{
    public record FullNameDTO(string firstName, string lastName, string? middleName = null);
}