using PetFamily.Application.Abstractions;
using PetFamily.Contracts.SpeciesAggregate.Requests;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWIthPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        GetFilteredSpeciesWithPaginationRequest Request) : IQuery;
}
