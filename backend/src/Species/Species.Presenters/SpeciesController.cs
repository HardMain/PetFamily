using Framework;
using Framework.Authorization;
using Framework.Envelopes;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using Species.Application.Commands.AddBreed;
using Species.Application.Commands.Create;
using Species.Application.Commands.Delete;
using Species.Application.Commands.DeleteBreed;
using Species.Application.Queries.GetBreedsBySpeciesIdWithPagination;
using Species.Application.Queries.GetFilteredSpeciesWIthPagination;
using Species.Contracts.Requests;

namespace Species.Presenters
{
    [Route("[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        [Permission(Permissions.Species.Create)]
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateSpeciesHandler handler,
            [FromBody] CreateSpeciesRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateSpeciesCommand(request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Species.Read)]
        [HttpGet]
        public async Task<ActionResult> GetFilteredSpecies(
            [FromServices] GetFilteredSpeciesWithPaginationHandler handler,
            [FromQuery] GetFilteredSpeciesWithPaginationRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetFilteredSpeciesWithPaginationQuery(request);

            var response = await handler.Handle(query, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Species.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteSpeciesHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteSpeciesCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }


        // ---------------------- breed ---------------------------

        [Permission(Permissions.Breeds.Create)]
        [HttpPost("{speciesId:guid}/breed")]
        public async Task<ActionResult> AddBreed(
            [FromRoute] Guid speciesId,
            [FromBody] AddBreedRequest request,
            [FromServices] AddBreedHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new AddBreedCommand(speciesId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Breeds.Read)]
        [HttpGet("{speciesId:guid}/breeds")]
        public async Task<ActionResult> GetBreedsBySpeciesId(
            [FromRoute] Guid speciesId,
            [FromServices] GetBreedsBySpeciesIdWithPaginationHandler handler,
            [FromQuery] GetBreedsBySpeciesIdWithPaginationRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetBreedsBySpeciesIdWithPaginationQuery(speciesId, request);

            var response = await handler.Handle(query, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Breeds.Delete)]
        [HttpDelete("{speciesId:guid}/breed/{breedId:guid}")]
        public async Task<ActionResult> DeleteBreed(
            [FromRoute] Guid speciesId,
            [FromRoute] Guid breedId,
            [FromServices] DeleteBreedHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteBreedCommand(speciesId, breedId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}