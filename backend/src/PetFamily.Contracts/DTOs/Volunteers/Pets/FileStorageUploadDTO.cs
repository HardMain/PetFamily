namespace PetFamily.Contracts.DTOs.Volunteers.Pets
{
    public record FileStorageUploadDTO(Stream Content, string ObjectName, string BucketName);
}