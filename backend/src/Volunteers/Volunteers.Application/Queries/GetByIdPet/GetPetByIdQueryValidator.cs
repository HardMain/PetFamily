using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Queries.GetByIdPet
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
