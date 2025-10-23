using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdatePetSupportStatus
{
    public record UpdatePetSupportStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetSupportStatusRequest Request) : ICommand;
}