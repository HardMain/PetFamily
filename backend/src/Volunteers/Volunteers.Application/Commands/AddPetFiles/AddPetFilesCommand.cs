using Core.Dtos;
using SharedKernel.Abstractions;

namespace Volunteers.Application.Commands.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<FileFormDto> Files) : ICommand;
}