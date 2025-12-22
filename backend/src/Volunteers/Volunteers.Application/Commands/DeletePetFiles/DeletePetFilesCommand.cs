using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.DeletePetFiles
{
    public record DeletePetFilesCommand(
        Guid VolunteerId, Guid PetId, DeletePetFilesRequest Request) : ICommand;
}
