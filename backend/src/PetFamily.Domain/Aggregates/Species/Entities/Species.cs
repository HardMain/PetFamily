using PetFamily.Domain.Aggregates.Species.ValueObjects;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.Species.Entities
{
    public class Species : Shared.Entity<SpeciesId>
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
                return "Name can not be empty!";

            return new Species(speciesId, name);
        }
    }
}
