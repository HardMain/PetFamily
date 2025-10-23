using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetById
{
    public record GetVolunteerByIdQuery(Guid Id) : IQuery;
}
