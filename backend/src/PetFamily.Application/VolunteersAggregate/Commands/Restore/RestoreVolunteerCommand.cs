using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Commands.Restore
{
    public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
}