using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.SetMainPhoto;

namespace PetFamily.Application.VolunteersAggregate.Commands.SetMainPhotoPet
{
    public record SetPetMainPhotoCommand(Guid VolunteerId, Guid PetId, SetPetMainPhotoRequest Request) : ICommand;
}