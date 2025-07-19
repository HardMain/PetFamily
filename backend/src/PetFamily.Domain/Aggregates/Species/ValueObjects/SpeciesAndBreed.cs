using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.Species.ValueObjects
{
    public record SpeciesAndBreed
    {
        private SpeciesAndBreed() { }

        private SpeciesAndBreed(SpeciesId speciesId, BreedId breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public SpeciesId SpeciesId { get; } = null!;
        public BreedId BreedId { get; } = null!;

        public static Result<SpeciesAndBreed> Create(SpeciesId speciesId, BreedId breedId)
        {
            return new SpeciesAndBreed(speciesId, breedId);
        }
    }
}
