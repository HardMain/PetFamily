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

namespace Species.Application.Queries.GetBreedsBySpeciesIdWithPagination
{
    public class GetBreedsBySpeciesIdWithPaginationHandler : IQueryHandler<PagedList<BreedReadDto>, GetBreedsBySpeciesIdWithPaginationQuery>
    {
        private readonly DistributedCacheEntryOptions _cacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        private readonly ISpeciesReadDbContext _speciesReadDbContext;
        private readonly ILogger<GetBreedsBySpeciesIdWithPaginationHandler> _logger;
        private readonly IValidator<GetBreedsBySpeciesIdWithPaginationQuery> _validator;
        private readonly ICacheService _cache;

        public GetBreedsBySpeciesIdWithPaginationHandler(
            ISpeciesReadDbContext speciesReadDbContext,
            ILogger<GetBreedsBySpeciesIdWithPaginationHandler> logger,
            IValidator<GetBreedsBySpeciesIdWithPaginationQuery> validator,
            ICacheService cache)
        {
            _speciesReadDbContext = speciesReadDbContext;
            _logger = logger;
            _validator = validator;
            _cache = cache;
        }

        public async Task<Result<PagedList<BreedReadDto>, ErrorList>> Handle(
            GetBreedsBySpeciesIdWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var key = $"species:{query.SpeciesId}" + 
                $":page:{query.Request.Page}" + 
                $":size:{query.Request.PageSize}";

            var breedsWithPagination = await _cache.GetOrSetAsync(
                key,
                _cacheOptions,
                async () =>
                {
                    var breedsQuery = _speciesReadDbContext.Breeds.Where(b => b.SpeciesId == query.SpeciesId);

                    return await breedsQuery
                        .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);
                },
                cancellationToken);

            if (breedsWithPagination is null)
            {
                return new PagedList<BreedReadDto>()
                {
                    Items = [],
                    TotalCount = 0,
                    Page = query.Request.Page,
                    PageSize = query.Request.PageSize,
                };
            }

            return breedsWithPagination;
        }
    }
}