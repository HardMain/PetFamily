using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Providers
{
    public interface IFileProvider
    {
        public Task<Result<IReadOnlyList<string>, ErrorList>> UploadFiles(
            IEnumerable<FileStorageUploadDto> filesData, CancellationToken cancellationToken = default);
        public Task<Result<IReadOnlyList<string>, ErrorList>> DeleteFiles(
            IEnumerable<FileStorageDeleteDto> filesData, CancellationToken cancellationToken = default);
        //public Task<Result<string>> GetFiles(FileMetadataDTO filesData, CancellationToken cancellationToken = default);
    }
}