using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.SpeciesManagement.Queries.GetFilteredSpeciesWIthPagination
{
    public class GetFilteredSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesReadDto>, GetFilteredSpeciesWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetFilteredSpeciesWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredSpeciesWithPaginationQuery> _validator;

        public GetFilteredSpeciesWithPaginationHandler(
            IReadDbContext readDbContext,
            ILogger<GetFilteredSpeciesWithPaginationHandler> logger,
            IValidator<GetFilteredSpeciesWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<PagedList<SpeciesReadDto>, ErrorList>> Handle(
            GetFilteredSpeciesWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var speciesQuery = _readDbContext.Species;

            speciesQuery = speciesQuery
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Name),
                    v => v.Name.Contains(query.Request.Name!))
                .SortByIf(!string.IsNullOrWhiteSpace(query.Request.SortBy),
                    query.Request.SortBy!,
                    query.Request.Ask);

            var breedsWithPagination = await speciesQuery
                .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);

            return breedsWithPagination;
        }
    }
}
