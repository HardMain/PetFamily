using Core.Abstractions;
using Volunteers.Contracts.Requests;

namespace Volunteers.Application.Commands.Create
{
    public record CreateVolunteerCommand(CreateVolunteerRequest Request) : ICommand;
}