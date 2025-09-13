namespace PetFamily.Contracts.Requests.Volunteers
{
    public record GetFilteredVolunteersWithPaginationRequest(
        string? Name,
        string? Email,
        string? Number, 
        string? SortBy,
        bool? Ask,
        int Page, 
        int PageSize);
}
