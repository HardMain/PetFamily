using Accounts.Domain.DataModels;
using Core.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.RegisterUser
{
    public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            UserManager<User> userManager, 
            ILogger<RegisterUserHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = User.CreateUser(command.Request.UserName, command.Request.Email);

            var result = await _userManager.CreateAsync(user, command.Request.Password);
            if (!result.Succeeded)
            {
                var errors = new ErrorList(result.Errors.Select(err => Error.Failure(err.Code, err.Description)));

                return errors;
            } 

            await _userManager.AddToRoleAsync(user, ParticipantAccount.PARTICIPANT);

            _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);

            return UnitResult<ErrorList>.Success();
        }
    }
}