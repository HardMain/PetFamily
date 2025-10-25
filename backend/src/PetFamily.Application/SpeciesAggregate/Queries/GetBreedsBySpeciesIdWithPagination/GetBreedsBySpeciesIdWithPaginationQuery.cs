using PetFamily.Application.Abstractions;
using PetFamily.Contracts.SpeciesAggregate.Requests;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetBreedsBySpeciesIdWithPagination
{
    public record GetBreedsBySpeciesIdWithPaginationQuery(
        Guid SpeciesId,
        GetBreedsBySpeciesIdWithPaginationRequest Request) : IQuery;
}
