using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Commands.RestorePet
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
