namespace PetFamily.Contracts.VolunteersAggregate.DTOs
{
    public record FullNameDto(string FirstName, string LastName, string? MiddleName = null);
}