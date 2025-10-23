using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.VolunteersAggregate.Commands.Create
{
    public record CreateVolunteerCommand(CreateVolunteerRequest Request) : ICommand;
}