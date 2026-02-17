using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Species.Application.Queries.GetBreedsBySpeciesIdWithPagination
{
    public class GetBreedsBySpeciesIdWithPaginationQueryValidator
        : AbstractValidator<GetBreedsBySpeciesIdWithPaginationQuery>
    {
        public GetBreedsBySpeciesIdWithPaginationQueryValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("speciesId"));

            RuleFor(b => b.Request.Page)
                .InclusiveBetween(1, 100)
                .WithError(Errors.General.ValueIsInvalid("page"));

            RuleFor(b => b.Request.PageSize)
                .InclusiveBetween(1, 50)
                .WithError(Errors.General.ValueIsInvalid("pageSize"));
        }
    }
}
