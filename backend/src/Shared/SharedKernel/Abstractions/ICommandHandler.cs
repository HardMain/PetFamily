using SharedKernel.Failures;

namespace SharedKernel.Abstractions
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