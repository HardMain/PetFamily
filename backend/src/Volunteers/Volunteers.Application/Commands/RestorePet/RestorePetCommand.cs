using Core.Abstractions;

namespace Volunteers.Application.Commands.RestorePet
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
