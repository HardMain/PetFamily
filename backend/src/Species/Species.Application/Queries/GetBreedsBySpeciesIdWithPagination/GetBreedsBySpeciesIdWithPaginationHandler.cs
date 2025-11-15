using Core.Models;
using FluentValidation;
using Framework.Extensions;
using Framework.Validation;
using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using Species.Application.Abstractions;
using Species.Contracts.DTOs;

namespace Species.Application.Queries.GetBreedsBySpeciesIdWithPagination
{
    public class GetBreedsBySpeciesIdWithPaginationHandler : IQueryHandler<PagedList<BreedReadDto>, GetBreedsBySpeciesIdWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _speciesReadDbContext;
        private readonly ILogger<GetBreedsBySpeciesIdWithPaginationHandler> _logger;
        private readonly IValidator<GetBreedsBySpeciesIdWithPaginationQuery> _validator;

        public GetBreedsBySpeciesIdWithPaginationHandler(
            ISpeciesReadDbContext speciesReadDbContext,
            ILogger<GetBreedsBySpeciesIdWithPaginationHandler> logger,
            IValidator<GetBreedsBySpeciesIdWithPaginationQuery> validator)
        {
            _speciesReadDbContext = speciesReadDbContext;
            _logger = logger;
            _validator = validator;
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

            var breedsQuery = _speciesReadDbContext.Breeds.Where(b => b.SpeciesId == query.SpeciesId);

            var breedsWithPagination = await breedsQuery
                .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);

            return breedsWithPagination;
        }
    }
}