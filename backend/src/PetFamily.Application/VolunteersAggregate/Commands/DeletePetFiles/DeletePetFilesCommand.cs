using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Commands.DeletePetFiles
{
    public record DeletePetFilesCommand(
        Guid VolunteerId, Guid PetId, DeletePetFilesRequest Request) : ICommand;
}
