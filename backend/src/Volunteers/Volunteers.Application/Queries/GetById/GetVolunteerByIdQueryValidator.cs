using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Queries.GetById
{
    public class GetVolunteerByIdQueryValidator : AbstractValidator<GetVolunteerByIdQuery>
    {
        public GetVolunteerByIdQueryValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("id"));
        }
    }
}
