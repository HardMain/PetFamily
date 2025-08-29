using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.DeletePetFiles
{
    public record DeletePetFilesCommand(
        Guid VolunteerId, Guid PetId, DeletePetFilesRequest Request);
}
