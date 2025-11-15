namespace Volunteers.Contracts.DTOs
{
    public record FullNameDto(string FirstName, string LastName, string? MiddleName = null);
}