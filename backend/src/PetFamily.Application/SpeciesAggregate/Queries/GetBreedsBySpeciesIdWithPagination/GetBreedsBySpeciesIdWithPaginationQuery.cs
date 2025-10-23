using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetBreedsBySpeciesIdWithPagination
{
    public record GetBreedsBySpeciesIdWithPaginationQuery(
        Guid SpeciesId,
        GetBreedsBySpeciesIdWithPaginationRequest Request) : IQuery;
}
