namespace PetFamily.Contracts.DTOs.Volunteers.Pets
{
    public record FileStorageUploadDto(Stream Content, string ObjectName, string BucketName);
}