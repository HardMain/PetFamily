using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Api.Controllers
{
    [Route("[controller]")]
    [ApiController] 
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteersHandler handler, 
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
    } 
}