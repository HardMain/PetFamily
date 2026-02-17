using Core.Abstractions;

namespace Volunteers.Application.Commands.Restore
{
    public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
}