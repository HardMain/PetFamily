namespace PetFamily.Contracts.DTOs.Volunteers.Pets
{
    public record FileDataDTO(Stream Stream, string BucketName, Guid ObjectName);
}