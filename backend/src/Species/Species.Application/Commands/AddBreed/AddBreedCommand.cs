using Core.Abstractions;
using Species.Contracts.Requests;

namespace Species.Application.Commands.AddBreed
{
    public record AddBreedCommand(
        Guid SpeciesId,
        AddBreedRequest Request) : ICommand;
}
