using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Application.Volunteers.PetsOperations.AddPets;
using PetFamily.Application.Volunteers.PetsOperations.DeletePets;
using PetFamily.Application.Volunteers.PetsOperations.GetPets;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> CreateFile(
            IFormFile file,
            //[FromBody] AddPetRequest request,
            [FromQuery] string bucketName,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken)
        {
            var metadata = new FileDataDTO(file.OpenReadStream(), bucketName, Guid.NewGuid());

            var command = new AddPetCommand(metadata);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpDelete("{objectName:guid}")]
        public async Task<ActionResult<string>> DeleteFile(
            [FromQuery] string bucketName,
            [FromRoute] Guid objectName,
            [FromServices] DeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var metadata = new FileMetadataDTO(bucketName, objectName);

            var command = new DeletePetCommand(metadata);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpGet("{objectName:guid}")]
        public async Task<ActionResult<string>> GetFile(
            [FromRoute] Guid objectName,
            [FromQuery] string bucketName,
            [FromServices] GetPetHandler handler,
            CancellationToken cancellationToken)
        {
            var metadata = new FileMetadataDTO(bucketName, objectName);

            var command = new GetPetCommand(metadata);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}