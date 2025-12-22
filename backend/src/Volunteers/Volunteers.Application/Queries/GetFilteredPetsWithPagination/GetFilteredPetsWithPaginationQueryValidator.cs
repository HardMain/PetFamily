using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Queries.GetFilteredPetsWithPagination
{
    public class GetFilteredPetsWithPaginationQueryValidator : AbstractValidator<GetFilteredPetsWithPaginationQuery>
    {
        private static readonly string[] AllowedSortFields =
        {
            "VolunteerId",
            "Name",
            "SpeciesAndBreed.SpeciesId",
            "SpeciesAndBreed.BreedId",
            "Color",
            "Address.City",
            "Address.HouseNumber",
            "Address.Country",
            "Address.Street",
            "WeightKg",
            "HeightCm",
            "OwnerPhone",
            "SupportStatus",
            "Id"
        };

        public GetFilteredPetsWithPaginationQueryValidator()
        {
            RuleFor(v => v.Request.Page)
                .InclusiveBetween(1, 100)
                .WithError(Errors.General.ValueIsInvalid("page"));

            RuleFor(v => v.Request.PageSize)
                .InclusiveBetween(1, 50)
                .WithError(Errors.General.ValueIsInvalid("pageSize"));

            RuleFor(v => v.Request.SortBy)
                .Must(sortBy => sortBy == null || AllowedSortFields.Contains(sortBy))
                .WithError(Errors.General.ValueIsInvalid("sortBy"));
        }
    }
}
