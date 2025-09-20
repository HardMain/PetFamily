using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species;

namespace PetFamily.Application.SpeciesManagement.Queries.GetFilteredSpeciesWIthPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        GetFilteredSpeciesWithPaginationRequest Request) : IQuery;
}
