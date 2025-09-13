using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.Restore
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
