using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Commands.DeletePet
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}