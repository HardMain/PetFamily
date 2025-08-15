using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly ILogger<MinioProvider> _logger;
        private readonly IMinioClient _minioClient;

        public MinioProvider(
            ILogger<MinioProvider> logger,
            IMinioClient minioClient)
        {
            _logger = logger;
            _minioClient = minioClient;
        }

        public async Task<Result<string>> UploadFile(
            FileDataDTO fileData, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs()
                    .WithBucket(fileData.BucketName);

                var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(fileData.BucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(fileData.Stream)
                    .WithObjectSize(fileData.Stream.Length)
                    .WithObject(fileData.ObjectName.ToString());

                var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return result.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload file in minio");

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }

        public async Task<Result<string>> DeleteFile(
            FileMetadataDTO fileData, CancellationToken cancellationToken = default)
        {
            try
            {
                var removeBucketArgs = new RemoveObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithObject(fileData.ObjectName.ToString());

                await _minioClient.RemoveObjectAsync(removeBucketArgs, cancellationToken);

                return fileData.ObjectName.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to delete file in minio");

                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string>> GetFile(
            FileMetadataDTO fileData, CancellationToken cancellationToken = default)
        {
            try
            {
                var putObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithObject(fileData.ObjectName.ToString())
                    .WithExpiry(60 * 60 * 24);

                var result = await _minioClient.PresignedGetObjectAsync(putObjectArgs);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to get URL file in minio");

                return Error.Failure("file.get.url", "Fail to get URL file in minio");
            }
        }
    }
}