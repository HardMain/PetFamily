using Core.Abstractions;
using Core.Caching;
using Core.Extensions;
using Core.Models;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using Species.Application.Abstractions;
using Species.Contracts.DTOs;

namespace Species.Application.Queries.GetFilteredSpeciesWIthPagination
{
    public class GetFilteredSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesReadDto>, GetFilteredSpeciesWithPaginationQuery>
    {
        private readonly DistributedCacheEntryOptions _cacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        private readonly ISpeciesReadDbContext _speciesReadDbContext;
        private readonly ILogger<GetFilteredSpeciesWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredSpeciesWithPaginationQuery> _validator;
        private readonly ICacheService _cache;

        public GetFilteredSpeciesWithPaginationHandler(
            ISpeciesReadDbContext readDbContext,
            ILogger<GetFilteredSpeciesWithPaginationHandler> logger,
            IValidator<GetFilteredSpeciesWithPaginationQuery> validator,
            ICacheService cache)
        {
            _speciesReadDbContext = readDbContext;
            _logger = logger;
            _validator = validator;
            _cache = cache;
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

            var key = $"species:name:{query.Request.Name ?? "all"}" +
                $":page:{query.Request.Page}" +
                $":size:{query.Request.PageSize}" +
                $":sortBy:{query.Request.SortBy}" +
                $":ask:{query.Request.Ask}";

            var speciesWithPagination = await _cache.GetOrSetAsync(
                key,
                _cacheOptions,
                async () =>
                {
                    speciesQuery = speciesQuery
                        .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Name),
                            v => v.Name.Contains(query.Request.Name!))
                        .SortByIf(!string.IsNullOrWhiteSpace(query.Request.SortBy),
                            query.Request.SortBy!,
                            query.Request.Ask);

                    return await speciesQuery
                        .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);
                },
                cancellationToken);

            if (speciesWithPagination is null)
            {
                return new PagedList<SpeciesReadDto>()
                {
                    Items = [],
                    TotalCount = 0,
                    Page = query.Request.Page,
                    PageSize = query.Request.PageSize,
                };
            }

            return speciesWithPagination;
        }
    }
}
