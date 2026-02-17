using Core.Abstractions;
using Core.Caching;
using Core.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Queries.GetByPetId
{
    public class GetPetByIdHandler : IQueryHandler<PetReadDto, GetPetByIdQuery>
    {
        private readonly DistributedCacheEntryOptions _cacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        };

        private readonly IVolunteersReadDbContext _readDbContext;
        private readonly ILogger<GetPetByIdHandler> _logger;
        private readonly IValidator<GetPetByIdQuery> _validator;
        private readonly ICacheService _cache;

        public GetPetByIdHandler(
            IValidator<GetPetByIdQuery> validator,
            ILogger<GetPetByIdHandler> logger,
            IVolunteersReadDbContext readDbContext,
            ICacheService cache)
        {
            _validator = validator;
            _logger = logger;
            _readDbContext = readDbContext;
            _cache = cache;
        }

        public async Task<Result<PetReadDto, ErrorList>> Handle(
            GetPetByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            string key = "pet:" + query.Id;

            var pet = await _cache.GetOrSetAsync(
                key,
                _cacheOptions,
                async () =>
                {
                    return await _readDbContext.Pets
                        .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
                },
                cancellationToken);

            if (pet == null)
            {
                _logger.LogWarning("Failed to get pet {PetId}", query.Id);

                return Errors.General.NotFound(query.Id).ToErrorList();
            }

            return pet;
        }
    }
}
