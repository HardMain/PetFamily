using Framework;
using Framework.Envelopes;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Application.Queries.GetByIdPet;
using Volunteers.Application.Queries.GetFilteredPetsWithPagination;
using Volunteers.Contracts.Requests;

namespace Volunteers.Presenters
{
    [Route("[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetFilteredPets(
            [FromServices] GetFilteredPetsWithPaginationHandler handler,
            [FromQuery] GetFilteredPetsWithPaginationRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetFilteredPetsWithPaginationQuery(request);

            var response = await handler.Handle(query, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetPetById(
            [FromRoute] Guid id,
            [FromServices] GetPetByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new GetPetByIdQuery(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}
