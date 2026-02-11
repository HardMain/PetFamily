using Core.Abstractions;
using Core.Dtos;

namespace Volunteers.Application.Commands.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<FileFormDto> Files) : ICommand;
}