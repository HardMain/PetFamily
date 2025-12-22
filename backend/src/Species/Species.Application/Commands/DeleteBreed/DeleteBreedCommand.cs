using SharedKernel.Abstractions;

namespace Species.Application.Commands.DeleteBreed
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}