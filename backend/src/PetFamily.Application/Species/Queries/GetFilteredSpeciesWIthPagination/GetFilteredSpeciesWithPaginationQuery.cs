using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species;

namespace PetFamily.Application.Species.Queries.GetFilteredSpeciesWIthPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        GetFilteredSpeciesWithPaginationRequest Request) : IQuery;
}
