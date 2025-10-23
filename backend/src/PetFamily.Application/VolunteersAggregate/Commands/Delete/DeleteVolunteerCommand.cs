using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
}