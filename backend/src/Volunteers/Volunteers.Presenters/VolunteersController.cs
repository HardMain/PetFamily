using Framework;
using Framework.Authorization;
using Framework.Envelopes;
using Framework.Processors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using Volunteers.Application.Commands.AddPet;
using Volunteers.Application.Commands.AddPetFiles;
using Volunteers.Application.Commands.Create;
using Volunteers.Application.Commands.Delete;
using Volunteers.Application.Commands.DeletePet;
using Volunteers.Application.Commands.DeletePetFiles;
using Volunteers.Application.Commands.MovePet;
using Volunteers.Application.Commands.Restore;
using Volunteers.Application.Commands.RestorePet;
using Volunteers.Application.Commands.SetMainPhotoPet;
using Volunteers.Application.Commands.UpdateDonationsInfo;
using Volunteers.Application.Commands.UpdateMainInfo;
using Volunteers.Application.Commands.UpdateMainInfoPet;
using Volunteers.Application.Commands.UpdatePetSupportStatus;
using Volunteers.Application.Commands.UpdateSocialNetworks;
using Volunteers.Application.Queries.GetById;
using Volunteers.Application.Queries.GetFilteredVolunteersWithPagination;
using Volunteers.Contracts.Requests;

namespace Volunteers.Presenters
{
    [Route("[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        // ----------- Volunteer -------------
        [Permission(Permissions.Volunteers.Create)]
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateVolunteerCommand(request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Read)]
        [HttpGet]
        public async Task<ActionResult> GetFilteredVolunteers(
            [FromServices] GetFilteredVolunteersWithPaginationHandler handler,
            [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetFilteredVolunteersWithPaginationQuery(request);

            var response = await handler.Handle(query, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Read)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetVolunteerById(
            [FromServices] GetVolunteerByIdHandler handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetVolunteerByIdQuery(id);

            var response = await handler.Handle(query, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Update)]
        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateMainInfoHandler handler,
            [FromBody] UpdateMainInfoRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateMainInfoCommand(id, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Update)]
        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromServices] UpdateSocialNetworksHandler handler,
            [FromBody] UpdateSocialNetworksRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateSocialNetworksCommand(id, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Update)]
        [HttpPut("{id:guid}/donations")]
        public async Task<ActionResult> UpdateDonationsInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateDonationsInfoHandler handler,
            [FromBody] UpdateDonationsInfoRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateDonationsInfoCommand(id, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Delete)]
        [HttpPut("{id:guid}/soft")]
        public async Task<ActionResult> SoftDelete(
            [FromRoute] Guid id,
            [FromServices] SoftDeleteVolunteerHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteVolunteerCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Volunteers.Restore)]
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

        [Permission(Permissions.Volunteers.Delete)]
        [HttpDelete("{id:guid}/hard")]
        public async Task<ActionResult> HardDelete(
            [FromRoute] Guid id,
            [FromServices] HardDeleteVolunteerHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteVolunteerCommand(id);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        // ----------- Pet -------------

        [Permission(Permissions.Pets.Create)]
        [HttpPost("{volunteerId:guid}/pet")]
        public async Task<ActionResult> AddPet(
            [FromRoute] Guid volunteerId,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new AddPetCommand(volunteerId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.ManageFiles)]
        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> AddPetFiles(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection fileCollection,
            [FromServices] AddPetFilesHandler handler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var filesFormDto = fileProcessor.Process(fileCollection);

            var command = new AddPetFilesCommand(volunteerId, petId, filesFormDto);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Update)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}")]
        public async Task<ActionResult> UpdatePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetRequest request,
            [FromServices] UpdatePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePetCommand(volunteerId, petId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Update)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}/support-status")]
        public async Task<ActionResult> UpdatePetSupportStatus(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetSupportStatusRequest request,
            [FromServices] UpdatePetSupportStatusHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePetSupportStatusCommand(volunteerId, petId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.ManageFiles)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}/main-photo")]
        public async Task<ActionResult> SetPetMainPhoto(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] SetPetMainPhotoRequest request,
            [FromServices] SetPetMainPhotoHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new SetPetMainPhotoCommand(volunteerId, petId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Delete)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}/soft")]
        public async Task<ActionResult> SoftDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] SoftDeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetCommand(volunteerId, petId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Restore)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}/restore")]
        public async Task<ActionResult> Restore(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] RestorePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RestorePetCommand(volunteerId, petId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Update)]
        [HttpPut("{volunteerId:guid}/pet/{petId:guid}/position")]
        public async Task<ActionResult> MovePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] MovePetRequest request,
            [FromServices] MovePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new MovePetCommand(volunteerId, petId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Delete)]
        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
        public async Task<ActionResult> HardDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] HardDeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetCommand(volunteerId, petId);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [Permission(Permissions.Pets.Delete)]
        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> DeletePetFiles(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] DeletePetFilesRequest request,
            [FromServices] DeletePetFilesHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetFilesCommand(volunteerId, petId, request);

            var response = await handler.Handle(command, cancellationToken);

            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}