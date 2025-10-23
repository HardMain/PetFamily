using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.DTOs;

namespace PetFamily.Application.VolunteersAggregate.Commands.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<PetFileFormDto> Files) : ICommand;
}