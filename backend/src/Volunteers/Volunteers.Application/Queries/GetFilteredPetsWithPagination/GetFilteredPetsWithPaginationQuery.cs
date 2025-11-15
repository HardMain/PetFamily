using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Queries.GetFilteredPetsWithPagination
{
    public record GetFilteredPetsWithPaginationQuery(
        GetFilteredPetsWithPaginationRequest Request) : IQuery;
}
