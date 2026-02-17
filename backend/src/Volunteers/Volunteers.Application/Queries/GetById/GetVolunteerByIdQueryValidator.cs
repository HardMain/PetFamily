using Core.Validation;
using FluentValidation;
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
