using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Api.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        private readonly List<PetFileFormDto> _fileDtos = [];

        public List<PetFileFormDto> Process(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var fileDto = new PetFileFormDto(stream, file.FileName);

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