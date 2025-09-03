﻿using Microsoft.Extensions.Logging;
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
        public const int MAX_DEGREE_OF_PARALLELISM = 10;

        private readonly ILogger<MinioProvider> _logger;
        private readonly IMinioClient _minioClient;

        public MinioProvider(
            ILogger<MinioProvider> logger,
            IMinioClient minioClient)
        {
            _logger = logger;
            _minioClient = minioClient;
        }

        public async Task<Result<IReadOnlyList<string>, ErrorList>> UploadFiles(
            IEnumerable<FileStorageUploadDTO> filesData, CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

            try
            { 
                await IfBucketNotExistCreateBucket(filesData.ToList(), cancellationToken);

                var tasks = filesData.Select(f => PutObject(f, semaphoreSlim, cancellationToken));

                var pathsResult = await Task.WhenAll(tasks);

                var failedResults = pathsResult
                    .Where(res => res.IsFailure)
                    .Select(res => res.Error);

                if (failedResults.Any())
                    return new ErrorList(failedResults);

                var result = pathsResult.Select(res => res.Value).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Some files were not uploaded to MinIO.");

                return Error.Failure(
                    "file.upload.minio",
                    "Some files were not uploaded to MinIO.").ToErrorList();
            }
        }

        public async Task<Result<IReadOnlyList<string>, ErrorList>> DeleteFiles
            (IEnumerable<FileStorageDeleteDTO> filesData, CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

            try
            {
                var tasks = filesData.Select(f => RemoveObject(f, semaphoreSlim, cancellationToken));

                var pathsResult = await Task.WhenAll(tasks);

                var failedResults = pathsResult
                    .Where(res => res.IsFailure)
                    .Select(res => res.Error);

                if (failedResults.Any())
                    return new ErrorList(failedResults);

                var result = pathsResult.Select(res => res.Value).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Some files have not been deleted from MinIO.");

                return Error.Failure(
                    "file.delete.minio",
                    "Some files may not have been deleted from MinIO.").ToErrorList();
            }
        }

        private async Task<Result<string>> PutObject(
            FileStorageUploadDTO fileStorageUpload,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileStorageUpload.BucketName)
                    .WithStreamData(fileStorageUpload.Content)
                    .WithObjectSize(fileStorageUpload.Content.Length)
                    .WithObject(fileStorageUpload.ObjectName);

                await _minioClient.PutObjectAsync(putObjectArgs);

                return fileStorageUpload.ObjectName;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, 
                    "Fail to upload file in minio with path {path} in bucket {bucket}", 
                    fileStorageUpload.ObjectName, 
                    fileStorageUpload.BucketName);

                return Error.Failure("file.upload", "fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task<Result<string>> RemoveObject(
            FileStorageDeleteDTO fileStorageDelete,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync();
             
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileStorageDelete.BucketName)
                    .WithObject(fileStorageDelete.ObjectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return fileStorageDelete.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to delete file in MinIO with objectName {objectName} in bucket {bucket}",
                    fileStorageDelete.ObjectName,
                    fileStorageDelete.BucketName);

                return Error.Failure("file.delete.minio", 
                    $"Fail to delete file in MinIO " +
                    $"with objectName {fileStorageDelete.ObjectName} " +
                    $"in bucket {fileStorageDelete.BucketName}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task IfBucketNotExistCreateBucket(
            IEnumerable<FileStorageUploadDTO> filesData,
            CancellationToken cancellationToken = default)
        {
            HashSet<string> bucketNames = filesData.Select(f => f.BucketName).ToHashSet();

            foreach (var bucketName in bucketNames)
            {
                var bucketExistsArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }
            }
        }

        //public async Task<Result<string>> GetFiles(
        //    FileMetadataDTO fileData, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var putObjectArgs = new PresignedGetObjectArgs()
        //            .WithBucket(fileData.BucketName)
        //            .WithObject(fileData.ObjectName.ToString())
        //            .WithExpiry(60 * 60 * 24);

        //        var result = await _minioClient.PresignedGetObjectAsync(putObjectArgs);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Fail to get URL file in minio");

        //        return Error.Failure("file.get.url", "Fail to get URL file in minio");
        //    }
        //}
    }
}