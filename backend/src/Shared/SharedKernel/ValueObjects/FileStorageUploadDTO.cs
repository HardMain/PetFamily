namespace SharedKernel.ValueObjects
{
    public record FileStorageUploadDto(Stream Content, string ObjectName, string BucketName);
}