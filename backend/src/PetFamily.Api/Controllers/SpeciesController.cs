using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Application.SpeciesAggregate.Commands.AddBreed;
using PetFamily.Application.SpeciesAggregate.Commands.Create;
using PetFamily.Application.SpeciesAggregate.Commands.Delete;
using PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed;
using PetFamily.Application.SpeciesAggregate.Queries.GetBreedsBySpeciesIdWithPagination;
using PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWIthPagination;
using PetFamily.Contracts.SpeciesAggregate.Requests;

namespace PetFamily.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
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