using Core.Abstractions;

namespace Volunteers.Application.Queries.GetById
{
    public record GetVolunteerByIdQuery(Guid Id) : IQuery;
}
