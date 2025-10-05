using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.SetMainPhoto
{
    public record SetPetMainPhotoCommand(Guid VolunteerId, Guid PetId, SetPetMainPhotoRequest Request) : ICommand;
}
