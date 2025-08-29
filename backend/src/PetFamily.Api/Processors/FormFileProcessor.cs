using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Api.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        private readonly List<FileFormDTO> _fileDTOs = [];

        public List<FileFormDTO> Process(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var fileDTO = new FileFormDTO(stream, file.FileName);

                _fileDTOs.Add(fileDTO);
            }

            return _fileDTOs;
        }

        public async ValueTask DisposeAsync()
        {
            foreach(var file in _fileDTOs)
            {
                await file.Content.DisposeAsync();
            }
        }
    }
}