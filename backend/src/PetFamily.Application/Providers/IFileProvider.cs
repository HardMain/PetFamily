using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Providers
{
    public interface IFileProvider
    {
        public Task<Result<string>> UploadFile(FileDataDTO fileData, CancellationToken cancellationToken = default);
        public Task<Result<string>> DeleteFile(FileMetadataDTO fileData, CancellationToken cancellationToken = default);
        public Task<Result<string>> GetFile(FileMetadataDTO fileData, CancellationToken cancellationToken = default);
    }
}