using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetByIdPet
{
    public record GetPetByIdQuery(Guid Id) : IQuery;
}