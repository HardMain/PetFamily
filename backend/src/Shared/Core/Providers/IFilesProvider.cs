using SharedKernel.Failures;
using SharedKernel.ValueObjects;

namespace Core.Providers
{
    public interface IFilesProvider
    {
        public Task<Result<IReadOnlyList<string>, ErrorList>> UploadFiles(
            IEnumerable<FileStorageUploadDto> filesData, CancellationToken cancellationToken = default);
        public Task<Result<IReadOnlyList<string>, ErrorList>> DeleteFiles(
            IEnumerable<FileStorageDeleteDto> filesData, CancellationToken cancellationToken = default);

        //public Task<Result<string>> GetFiles(FileMetadataDTO filesData, CancellationToken cancellationToken = default);
    }
}