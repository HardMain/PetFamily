using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Commands.Restore
{
    public record RestoreVolunteerCommand(Guid VolunteerId) : ICommand;
}