namespace Species.Contracts
{
    public interface ISpeciesContract
    {
        Task<bool> BreedExistsInSpeciesAsync(Guid breedId, Guid speciesId);
    }
}
