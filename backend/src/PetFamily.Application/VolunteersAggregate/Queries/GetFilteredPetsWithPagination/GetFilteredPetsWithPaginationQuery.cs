using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetFilteredPetsWithPagination
{
    public record GetFilteredPetsWithPaginationQuery(
        GetFilteredPetsWithPaginationRequest Request) : IQuery;
}
