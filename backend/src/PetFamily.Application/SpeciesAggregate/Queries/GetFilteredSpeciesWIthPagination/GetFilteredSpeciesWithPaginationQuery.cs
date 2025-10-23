using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWIthPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        GetFilteredSpeciesWithPaginationRequest Request) : IQuery;
}
