using Core.Abstractions;

namespace Volunteers.Application.Queries.GetByPetId
{
    public record GetPetByIdQuery(Guid Id) : IQuery;
}