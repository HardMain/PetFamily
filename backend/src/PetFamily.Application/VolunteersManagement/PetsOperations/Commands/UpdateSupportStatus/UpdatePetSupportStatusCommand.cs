using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.UpdateSupportStatus
{
    public record UpdatePetSupportStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetSupportStatusRequest Request) : ICommand;
}