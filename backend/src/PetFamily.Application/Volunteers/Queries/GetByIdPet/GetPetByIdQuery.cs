using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetByIdPet
{
    public record GetPetByIdQuery(Guid Id) : IQuery;
}