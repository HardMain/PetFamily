using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Queries.GetFilteredPetsWithPagination
{
    public class GetFilteredPetsWithPaginationHandler 
        : IQueryHandler<PagedList<PetReadDto>, GetFilteredPetsWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetFilteredPetsWithPaginationHandler> _logger;
        private readonly IValidator<GetFilteredPetsWithPaginationQuery> _validator;

        public GetFilteredPetsWithPaginationHandler(
            IReadDbContext readDbContext,
            ILogger<GetFilteredPetsWithPaginationHandler> logger,
            IValidator<GetFilteredPetsWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<PagedList<PetReadDto>, ErrorList>> Handle(
            GetFilteredPetsWithPaginationQuery query, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var petsQuery = _readDbContext.Pets;

            petsQuery = petsQuery
                .WhereIf(query.Request.VolunteerId != null,
                    p => p.VolunteerId == query.Request.VolunteerId)
                .WhereIf(query.Request.SpeciesId != null,
                    p => p.SpeciesAndBreed.SpeciesId == query.Request.SpeciesId)
                .WhereIf(query.Request.BreedId != null,
                    p => p.SpeciesAndBreed.BreedId == query.Request.BreedId)
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.City),
                    p => p.Address.City.Contains(query.Request.City!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.HouseNumber),
                    p => p.Address.HouseNumber.Contains(query.Request.HouseNumber!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Country),
                    p => p.Address.Country.Contains(query.Request.Country!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Street),
                    p => p.Address.Street.Contains(query.Request.Street!))
                .WhereIf(query.Request.MinWeightKg != null,
                    p => p.WeightKg >= query.Request.MinWeightKg)
                .WhereIf(query.Request.MaxWeightKg != null,
                    p => p.WeightKg <= query.Request.MaxWeightKg)
                .WhereIf(query.Request.MinHeightCm != null,
                    p => p.HeightCm >= query.Request.MinHeightCm)
                .WhereIf(query.Request.MaxHeightCm != null,
                    p => p.HeightCm <= query.Request.MaxHeightCm)
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.OwnerPhone),
                    p => p.OwnerPhone.Contains(query.Request.OwnerPhone!))
                .WhereIf(query.Request.isCastrated != null,
                    p => p.isCastrated == query.Request.isCastrated)
                .WhereIf(query.Request.isVaccinated != null,
                    p => p.isVaccinated == query.Request.isVaccinated)
                .WhereIf(!string.IsNullOrWhiteSpace(query.Request.SupportStatus),
                    p => p.SupportStatus.Contains(query.Request.SupportStatus!))
                .SortByIf(!string.IsNullOrWhiteSpace(query.Request.SortBy),
                    query.Request.SortBy!,
                    query.Request.Ask);

            var petsWithPagination = await petsQuery
                .ToPagedList(query.Request.Page, query.Request.PageSize, cancellationToken);

            return petsWithPagination;
        }
    }
}
