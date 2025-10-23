using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.DeleteBreed
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}