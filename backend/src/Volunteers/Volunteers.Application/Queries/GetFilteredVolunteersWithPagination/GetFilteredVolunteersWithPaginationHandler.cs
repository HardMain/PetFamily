using Core.Abstractions;
using Core.Extensions;
using Core.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Queries.GetFilteredVolunteersWithPagination
{
    public class GetFilteredVolunteersWithPaginationHandler
        : IQueryHandler<PagedList<VolunteerReadDto>, GetFilteredVolunteersWithPaginationQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;
        private readonly ILogger<GetFilteredVolunteersWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;

        public GetFilteredVolunteersWithPaginationHandler(
            IVolunteersReadDbContext readDbContext,
            ILogger<GetFilteredVolunteersWithPaginationHandler> logger,
            IValidator<GetFilteredVolunteersWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<PagedList<VolunteerReadDto>, ErrorList>> Handle(
            GetFilteredVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteersQuery = _readDbContext.Volunteers;

            volunteersQuery = volunteersQuery
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Number),
                    v => v.PhoneNumber.Contains(query.Request.Number!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Email),
                    v => v.Email.Contains(query.Request.Email!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Name),
                    v => string.Concat(
                        v.FullName.FirstName + " ",
                        v.FullName.MiddleName + " ",
                        v.FullName.LastName)
                    .Contains(query.Request.Name!))
                .SortByIf(!string.IsNullOrWhiteSpace(query.Request.SortBy),
                    query.Request.SortBy!,
                    query.Request.Ask);

            var volunteersWithPagination = await volunteersQuery
                .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);

            return volunteersWithPagination;
        }
    }
}