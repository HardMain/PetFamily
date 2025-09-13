using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Abstractions
{
    public interface ICommandHandler<TResponse, TCommand> where TCommand : ICommand
    {
        public Task<Result<TResponse, ErrorList>> Handle(
            TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        public Task<Result<ErrorList>> Handle(
            TCommand command, CancellationToken cancellationToken = default);
    }
}