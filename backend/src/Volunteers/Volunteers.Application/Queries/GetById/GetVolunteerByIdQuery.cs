using SharedKernel.Abstractions;

namespace Volunteers.Application.Queries.GetById
{
    public record GetVolunteerByIdQuery(Guid Id) : IQuery;
}
