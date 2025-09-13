using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.Delete
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}