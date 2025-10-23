using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetById
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
