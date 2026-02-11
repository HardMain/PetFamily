using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.LoginUser
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(lc => lc.Request.Email)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(lc => lc.Request.Password)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}