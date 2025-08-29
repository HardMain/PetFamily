using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.AddPetFiles
{
    public record AddPetFilesCommand(
        Guid VolunteerId, Guid PetId, IEnumerable<FileFormDTO> Files);
}