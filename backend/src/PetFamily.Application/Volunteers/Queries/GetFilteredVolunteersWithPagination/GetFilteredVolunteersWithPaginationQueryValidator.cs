using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination
{
    public class GetFilteredVolunteersWithPaginationQueryValidator :
        AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
    {
        private static readonly string[] AllowedSortFields =
        {
            "FullName.FirstName",
            "FullName.LastName",
            "FullName.MiddleName",
            "Email",
            "PhoneNumber",
            "Id",
            "ExperienceYears"
        };

        public GetFilteredVolunteersWithPaginationQueryValidator()
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