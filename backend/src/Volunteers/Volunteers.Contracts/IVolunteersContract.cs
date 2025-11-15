namespace Volunteers.Contracts
{
    public interface IVolunteersContract
    {
        Task<bool> IsBreedInUseAsync(Guid BreedId);
        Task<bool> IsSpeciesInUseAsync(Guid SpeciesId);
    }
}
