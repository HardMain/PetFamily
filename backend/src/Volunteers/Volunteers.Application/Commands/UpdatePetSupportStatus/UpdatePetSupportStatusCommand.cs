using SharedKernel.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.UpdatePetSupportStatus
{
    public record UpdatePetSupportStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        UpdatePetSupportStatusRequest Request) : ICommand;
}