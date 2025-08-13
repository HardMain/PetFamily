using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Aggregates.Species.Entities
{
    public class Breed : Entity<BreedId>
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
                return Errors.General.ValueIsInvalid("name");

            return new Breed(breedId, name);
        }
    }
}
