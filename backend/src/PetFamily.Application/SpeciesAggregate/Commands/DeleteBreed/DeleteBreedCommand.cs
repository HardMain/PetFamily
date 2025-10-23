using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}