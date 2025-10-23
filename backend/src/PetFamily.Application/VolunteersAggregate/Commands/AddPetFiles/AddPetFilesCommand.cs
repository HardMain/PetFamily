using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Commands.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<FileFormDto> Files) : ICommand;
}