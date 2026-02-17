using Core.Abstractions;

namespace Species.Application.Commands.DeleteBreed
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}