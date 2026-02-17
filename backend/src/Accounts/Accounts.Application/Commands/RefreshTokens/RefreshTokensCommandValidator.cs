using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Accounts.Application.Commands.RefreshTokens
{
    public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
    {
        public RefreshTokensCommandValidator()
        {
            RuleFor(rc => rc.AccessToken)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(rc => rc.RefreshToken)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}