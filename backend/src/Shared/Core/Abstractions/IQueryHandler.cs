using SharedKernel.Failures;

namespace Core.Abstractions
{
    public interface IQueryHandler<TResponse, TQuery> where TQuery : IQuery
    {
        public Task<Result<TResponse, ErrorList>> Handle(
            TQuery query, CancellationToken cancellationToken = default);
    }
}