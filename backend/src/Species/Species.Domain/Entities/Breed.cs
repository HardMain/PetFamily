using SharedKernel.Abstractions;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;

namespace Species.Domain.Entities
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