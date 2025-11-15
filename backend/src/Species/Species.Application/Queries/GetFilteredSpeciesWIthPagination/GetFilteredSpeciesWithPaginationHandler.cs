using Core.Models;
using FluentValidation;
using Framework.Extensions;
using Framework.Validation;
using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using Species.Application.Abstractions;
using Species.Contracts.DTOs;

namespace Species.Application.Queries.GetFilteredSpeciesWIthPagination
{
    public class GetFilteredSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesReadDto>, GetFilteredSpeciesWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _speciesReadDbContext;
        private readonly ILogger<GetFilteredSpeciesWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredSpeciesWithPaginationQuery> _validator;

        public GetFilteredSpeciesWithPaginationHandler(
            ISpeciesReadDbContext readDbContext,
            ILogger<GetFilteredSpeciesWithPaginationHandler> logger,
            IValidator<GetFilteredSpeciesWithPaginationQuery> validator)
        {
            _speciesReadDbContext = readDbContext;
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

            var speciesQuery = _speciesReadDbContext.Species;

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
