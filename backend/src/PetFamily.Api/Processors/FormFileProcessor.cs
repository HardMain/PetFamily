using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Api.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        private readonly List<FileFormDto> _fileDtos = [];

        public List<FileFormDto> Process(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var fileDto = new FileFormDto(stream, file.FileName);

                _fileDtos.Add(fileDto);
            }

            return _fileDtos;
        }

        public async ValueTask DisposeAsync()
        {
            foreach(var file in _fileDtos)
            {
                await file.Content.DisposeAsync();
            }
        }
    }
}