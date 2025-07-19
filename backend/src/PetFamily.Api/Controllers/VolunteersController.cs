using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;

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

            var result = await handler.Handle(command, cancellationToken);

            return result.ToResponse();
        }
    } 
}