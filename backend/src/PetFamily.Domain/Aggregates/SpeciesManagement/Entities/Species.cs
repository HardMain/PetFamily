using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Aggregates.Species.Entities
{
    public class Species : Entity<SpeciesId>
    {
        private readonly List<Breed> _breeds = [];

        private Species(SpeciesId id) : base(id) { }

        private Species(SpeciesId speciesId, string name) : base(speciesId)
        {
            Name = name;
        }

        public string Name { get; private set; } = default!;
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species> Create(SpeciesId speciesId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid("name");

            return new Species(speciesId, name);
        }

        public void AddBreed(Breed breed)
        {
            _breeds.Add(breed);
        }
    }
}
