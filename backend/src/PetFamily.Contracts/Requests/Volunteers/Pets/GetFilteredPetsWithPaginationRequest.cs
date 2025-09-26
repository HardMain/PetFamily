namespace PetFamily.Contracts.Requests.Volunteers.Pets
{
    public record GetFilteredPetsWithPaginationRequest(
        Guid? VolunteerId,
        string? Name,
        Guid? SpeciesId,
        Guid? BreedId,
        string? Street, 
        string? HouseNumber, 
        string? City, 
        string? Country,
        double? MinWeightKg,
        double? MaxWeightKg,
        double? MinHeightCm,
        double? MaxHeightCm,
        string? OwnerPhone,
        bool? isCastrated,
        bool? isVaccinated,
        string? SupportStatus,
        string? SortBy,
        bool? Ask,
        int Page,
        int PageSize
    );
}