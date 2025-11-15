using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Queries.GetFilteredVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        GetFilteredVolunteersWithPaginationRequest Request) : IQuery;
}