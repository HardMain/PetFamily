using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.FilesOperations.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<FileFormDto> Files) : ICommand;
}