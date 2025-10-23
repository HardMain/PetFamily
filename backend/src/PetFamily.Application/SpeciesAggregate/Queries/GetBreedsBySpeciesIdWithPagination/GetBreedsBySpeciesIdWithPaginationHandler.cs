using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetBreedsBySpeciesIdWithPagination
{
    public class GetBreedsBySpeciesIdWithPaginationHandler : IQueryHandler<PagedList<BreedReadDto>, GetBreedsBySpeciesIdWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetBreedsBySpeciesIdWithPaginationHandler> _logger;
        private readonly IValidator<GetBreedsBySpeciesIdWithPaginationQuery> _validator;

        public GetBreedsBySpeciesIdWithPaginationHandler(
            IReadDbContext readDbContext,
            ILogger<GetBreedsBySpeciesIdWithPaginationHandler> logger,
            IValidator<GetBreedsBySpeciesIdWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
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

            var breedsQuery = _readDbContext.Breeds.Where(b => b.SpeciesId == query.SpeciesId);

            var breedsWithPagination = await breedsQuery
                .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);

            return breedsWithPagination;
        }
    }
}