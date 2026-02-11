using Core.Abstractions;
using Species.Contracts.Requests;

namespace Species.Application.Queries.GetBreedsBySpeciesIdWithPagination
{
    public record GetBreedsBySpeciesIdWithPaginationQuery(
        Guid SpeciesId,
        GetBreedsBySpeciesIdWithPaginationRequest Request) : IQuery;
}
