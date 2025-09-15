using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
}