using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Queries.GetById
{
    public record GetPetByIdQuery(Guid Id) : IQuery;
}