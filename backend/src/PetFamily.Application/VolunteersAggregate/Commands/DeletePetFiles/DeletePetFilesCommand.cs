using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.DeletePetFiles
{
    public record DeletePetFilesCommand(
        Guid VolunteerId, Guid PetId, DeletePetFilesRequest Request) : ICommand;
}
