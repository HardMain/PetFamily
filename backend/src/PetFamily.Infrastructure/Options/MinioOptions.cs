namespace PetFamily.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string MINIO = "Minio";
        public string Endpoint { get; init; } = default!;
        public string AccessKey { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public bool WithSSL { get; init; } = false;
    }
}