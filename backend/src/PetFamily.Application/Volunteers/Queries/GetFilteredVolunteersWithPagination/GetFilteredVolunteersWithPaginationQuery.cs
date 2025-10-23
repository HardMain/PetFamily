using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        GetFilteredVolunteersWithPaginationRequest Request) : IQuery;
}