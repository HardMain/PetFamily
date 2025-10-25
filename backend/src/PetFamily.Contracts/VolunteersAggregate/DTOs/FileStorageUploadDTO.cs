namespace PetFamily.Contracts.VolunteersAggregate.DTOs
{
    public record FileStorageUploadDto(Stream Content, string ObjectName, string BucketName);
}