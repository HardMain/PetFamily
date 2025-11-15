using SharedKernel.Abstractions;
using Species.Contracts.Requests;

namespace Species.Application.Commands.Create
{
    public record CreateSpeciesCommand(
        CreateSpeciesRequest Request) : ICommand;
}