using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.SpeciesManagement.Queries.GetFilteredSpeciesWIthPagination
{
    public class GetFilteredSpeciesWithPaginationQueryValidator
        : AbstractValidator<GetFilteredSpeciesWithPaginationQuery>
    {
        private static readonly string[] AllowedSortFields =
        {
            "Name",
            "Id",
        };

        public GetFilteredSpeciesWithPaginationQueryValidator()
        {
            RuleFor(s => s.Request.Page)
                .InclusiveBetween(1, 100)
                .WithError(Errors.General.ValueIsInvalid("page"));

            RuleFor(s => s.Request.PageSize)
                .InclusiveBetween(1, 50)
                .WithError(Errors.General.ValueIsInvalid("pageSize"));

            RuleFor(s => s.Request.SortBy)
                .Must(sortBy => sortBy == null || AllowedSortFields.Contains(sortBy))
                .WithError(Errors.General.ValueIsInvalid("sortBy"));
        }
    }
}
