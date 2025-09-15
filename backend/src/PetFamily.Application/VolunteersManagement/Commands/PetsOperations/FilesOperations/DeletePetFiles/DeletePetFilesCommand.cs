using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.FilesOperations.DeletePetFiles
{
    public record DeletePetFilesCommand(
        Guid VolunteerId, Guid PetId, DeletePetFilesRequest Request) : ICommand;
}
