namespace Species.Contracts.Requests
{
    public record GetFilteredSpeciesWithPaginationRequest(
        string? Name,
        string? SortBy,
        bool? Ask,
        int Page,
        int PageSize);
}
