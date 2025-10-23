using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetFilteredPetsWithPagination
{
    public record GetFilteredPetsWithPaginationQuery(
        GetFilteredPetsWithPaginationRequest Request) : IQuery;
}
