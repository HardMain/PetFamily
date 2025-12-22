using SharedKernel.Abstractions;

namespace Volunteers.Application.Commands.Restore
{
    public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
}