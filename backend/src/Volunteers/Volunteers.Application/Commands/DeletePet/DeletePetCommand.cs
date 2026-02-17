using Core.Abstractions;

namespace Volunteers.Application.Commands.DeletePet
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}