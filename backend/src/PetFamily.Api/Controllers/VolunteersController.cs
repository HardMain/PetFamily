using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateDonationsInfo;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Api.Controllers
{
    [Route("[controller]")]
    [ApiController] 
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler, 
            [FromBody] CreateVolunteerRequest request, 
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand();

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult<Guid>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateMainInfoHandler handler,
            [FromBody] UpdateMainInfoRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult<Guid>> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromServices] UpdateSocialNetworksHandler handler,
            [FromBody] UpdateSocialNetworksRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
        [HttpPut("{id:guid}/donations")]
        public async Task<ActionResult<Guid>> UpdateDonationsInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateDonationsInfoHandler handler,
            [FromBody] UpdateDonationsInfoRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    } 
}