using PetFamily.Domain.Aggregates.Species.ValueObjects;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.Species.Entities
{
    public class Breed : Shared.Entity<BreedId>
    {
        private Breed(BreedId id) : base(id) { }

        private Breed(BreedId breedId, string name) : base(breedId)
        {
            Name = name;
        }

        public string Name { get; private set; } = default!;

        public static Result<Breed> Create(BreedId breedId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name can not be empty!";

            return new Breed(breedId, name);
        }
    }
}
