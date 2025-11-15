using SharedKernel.Abstractions;

namespace Volunteers.Application.Queries.GetByIdPet
{
    public record GetPetByIdQuery(Guid Id) : IQuery;
}