using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.RestorePet
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
