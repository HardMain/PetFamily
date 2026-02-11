using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(rc => rc.Request.Email)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(rc => rc.Request.Password)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(rc => rc.Request.UserName)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}