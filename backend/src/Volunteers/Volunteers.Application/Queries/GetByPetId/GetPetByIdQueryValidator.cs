using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Volunteers.Application.Queries.GetByPetId
{
    public class GetPetByIdQueryValidator : AbstractValidator<GetPetByIdQuery>
    {
        public GetPetByIdQueryValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("id"));
        }
    }
}
