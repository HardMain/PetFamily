using PetFamily.Application.Abstractions;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Application.VolunteersAggregate.Commands.Create
{
    public record CreateVolunteerCommand(CreateVolunteerRequest Request) : ICommand;
}