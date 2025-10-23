using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetFilteredVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        GetFilteredVolunteersWithPaginationRequest Request) : IQuery;
}