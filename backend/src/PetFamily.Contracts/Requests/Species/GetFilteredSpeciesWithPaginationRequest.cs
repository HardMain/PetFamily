namespace PetFamily.Contracts.Requests.Species
{
    public record GetFilteredSpeciesWithPaginationRequest(
        string? Name,
        string? SortBy,
        bool? Ask,
        int Page,
        int PageSize);
}
