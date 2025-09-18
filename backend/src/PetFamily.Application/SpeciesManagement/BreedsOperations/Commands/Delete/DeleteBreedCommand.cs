using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.BreedsOperations.Commands.Delete
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}