using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Application.Volunteers.Queries.GetByIdPet;
using PetFamily.Application.Volunteers.Queries.GetFilteredPetsWithPagination;
using PetFamily.Application.VolunteersManagement.PetsOperations.Queries.GetById;
using PetFamily.Application.VolunteersManagement.PetsOperations.Queries.GetFilteredPetsWithPagination;
using PetFamily.Application.VolunteersManagement.Queries.GetByIdPet;
using PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Api.Controllers
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
