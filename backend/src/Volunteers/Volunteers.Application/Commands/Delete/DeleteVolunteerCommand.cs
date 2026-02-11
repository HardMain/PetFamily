using Core.Abstractions;

namespace Volunteers.Application.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
}