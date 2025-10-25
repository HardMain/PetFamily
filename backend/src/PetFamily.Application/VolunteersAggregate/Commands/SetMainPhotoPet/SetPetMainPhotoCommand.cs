using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.SetMainPhotoPet
{
    public record SetPetMainPhotoCommand(Guid VolunteerId, Guid PetId, SetPetMainPhotoRequest Request) : ICommand;
}