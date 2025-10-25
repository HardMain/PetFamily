using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Contracts.VolunteersAggregate.DTOs;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetFilteredVolunteersWithPagination
{
    public class GetFilteredVolunteersWithPaginationHandler
        : IQueryHandler<PagedList<VolunteerReadDto>, GetFilteredVolunteersWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetFilteredVolunteersWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;

        public GetFilteredVolunteersWithPaginationHandler(
            IReadDbContext readDbContext,
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