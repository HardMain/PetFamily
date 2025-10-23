using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpeciesIdWithPagination
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
