using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.SpeciesManagement.BreedsOperations.Queries.GetBreedsBySpeciesIdWIthPagination
{
    public record GetBreedsBySpeciesIdWithPaginationQuery(
        Guid SpeciesId,
        GetBreedsBySpeciesIdWithPaginationRequest Request) : IQuery;
}
