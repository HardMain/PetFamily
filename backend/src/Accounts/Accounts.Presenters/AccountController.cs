using Accounts.Application.Commands.LoginUser;
using Accounts.Application.Commands.RefreshTokens;
using Accounts.Application.Commands.RegisterUser;
using Accounts.Contracts.Requests;
using Framework;
using Framework.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Presenters
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("test1")]
        public IActionResult Test1()
        {
            return Ok();
        }

        [HttpPost("test2")]
        public IActionResult Test2()
        {
            return Ok();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request,
            [FromServices] RegisterUserHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserRequest request,
            [FromServices] LoginHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request);

            var response = await handler.Handle(command, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens(
            [FromBody] RefreshTokensRequest request,
            [FromServices] RefreshTokensHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);

            var response = await handler.Handle(command, cancellationToken);
            if (response.IsFailure)
                return response.Error.ToResponse();

            var envelope = Envelope.Ok(response.Value);

            return Ok(envelope);
        }
    }
}