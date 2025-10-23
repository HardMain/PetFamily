using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpeciesIdWithPagination
{
    public record GetBreedsBySpeciesIdWithPaginationQuery(
        Guid SpeciesId,
        GetBreedsBySpeciesIdWithPaginationRequest Request) : IQuery;
}
