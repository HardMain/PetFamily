using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Restore
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
