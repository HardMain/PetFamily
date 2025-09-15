using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Abstractions
{
    public interface IQueryHandler<TResponse, TQuery> where TQuery : IQuery
    {
        public Task<Result<TResponse, ErrorList>> Handle(
            TQuery query, CancellationToken cancellationToken = default);
    }
}