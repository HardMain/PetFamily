using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Envelopes;
using PetFamily.Api.Extensions;
using PetFamily.Api.Processors;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteersOperations.Create;
using PetFamily.Application.VolunteersOperations.HardDelete;
using PetFamily.Application.VolunteersOperations.PetsOperations.Add;
using PetFamily.Application.VolunteersOperations.PetsOperations.Delete;
using PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.AddPetFiles;
using PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.DeletePetFiles;
using PetFamily.Application.VolunteersOperations.Restore;
using PetFamily.Application.VolunteersOperations.SoftDelete;
using PetFamily.Application.VolunteersOperations.UpdateDonationsInfo;
using PetFamily.Application.VolunteersOperations.UpdateMainInfo;
using PetFamily.Application.VolunteersOperations.UpdateSocialNetworks;
using PetFamily.Contracts.Requests.Volunteers;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        // ----------- Volunteer -------------

        [HttpPost]
        public async Task<ActionResult> Create(
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
        public async Task<ActionResult> UpdateMainInfo(
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
        public async Task<ActionResult> UpdateSocialNetworks(
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
        public async Task<ActionResult> UpdateDonationsInfo(
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

        [HttpPut("{id:guid}/restore")]
        public async Task<ActionResult> Restore(
            [FromRoute] Guid id,
            [FromServices] RestoreVolunteerHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RestoreVolunteerCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpDelete("{id:guid}/soft")]
        public async Task<ActionResult> SoftDelete(
            [FromRoute] Guid id,
            [FromServices] SoftDeleteVolunteerHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new SoftDeleteVolunteerCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpDelete("{id:guid}/hard")]
        public async Task<ActionResult> HardDelete(
            [FromRoute] Guid id,
            [FromServices] HardDeleteVolunteerHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new HardDeleteVolunteerCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        // ----------- Pet -------------

        [HttpPost("{id:guid}/pet")]
        public async Task<ActionResult> AddPet(
            [FromRoute] Guid id,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> AddPetFiles(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection fileCollection,
            [FromServices] AddPetFilesHandler handler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var filesFormDTO = fileProcessor.Process(fileCollection);

            var command = new AddPetFilesCommand(volunteerId, petId, filesFormDTO);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}")]
        public async Task<ActionResult> DeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] DeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetCommand(volunteerId, petId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> DeletePetFiles(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] DeletePetFilesRequest request,
            [FromServices] DeletePetFilesHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(volunteerId, petId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}