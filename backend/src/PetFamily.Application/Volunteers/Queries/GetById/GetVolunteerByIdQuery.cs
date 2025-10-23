using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetById
{
    public record GetVolunteerByIdQuery(Guid Id) : IQuery;
}
