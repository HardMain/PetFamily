using Core.Abstractions;
using Species.Contracts.Requests;

namespace Species.Application.Queries.GetFilteredSpeciesWIthPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        GetFilteredSpeciesWithPaginationRequest Request) : IQuery;
}
