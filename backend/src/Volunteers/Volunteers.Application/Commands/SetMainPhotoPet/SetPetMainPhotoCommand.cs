using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.SetMainPhotoPet
{
    public record SetPetMainPhotoCommand(Guid VolunteerId, Guid PetId, SetPetMainPhotoRequest Request) : ICommand;
}