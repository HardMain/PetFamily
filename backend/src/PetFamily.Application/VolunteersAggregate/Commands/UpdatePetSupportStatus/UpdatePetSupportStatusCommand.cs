using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdatePetSupportStatus
{
    public record UpdatePetSupportStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetSupportStatusRequest Request) : ICommand;
}