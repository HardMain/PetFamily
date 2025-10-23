using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.Restore
{
    public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
}