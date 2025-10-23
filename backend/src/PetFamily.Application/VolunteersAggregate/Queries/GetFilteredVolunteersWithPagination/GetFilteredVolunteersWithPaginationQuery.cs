using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetFilteredVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        GetFilteredVolunteersWithPaginationRequest Request) : IQuery;
}