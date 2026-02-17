using SharedKernel.Failures;

namespace Core.Abstractions
{
    public interface ICommandHandler<TResponse, TCommand> where TCommand : ICommand
    {
        public Task<Result<TResponse, ErrorList>> Handle(
            TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        public Task<UnitResult<ErrorList>> Handle(
            TCommand command, CancellationToken cancellationToken = default);
    }
}