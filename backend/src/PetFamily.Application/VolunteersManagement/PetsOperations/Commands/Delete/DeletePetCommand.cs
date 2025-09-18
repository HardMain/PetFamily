using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Delete
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}