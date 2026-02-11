using Core.Abstractions;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;

namespace Species.Domain.Entities
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

        public Breed AddBreed(Breed breed)
        {
            _breeds.Add(breed);

            return breed;
        }

        public Breed DeleteBreed(Breed breed)
        {
            _breeds.Remove(breed);

            return breed;
        }

        public Result<Breed> GetBreedById(BreedId breedId)
        {
            var breed = _breeds.FirstOrDefault(b => b.Id == breedId);
            if (breed == null)
                return Errors.General.NotFound(breedId);

            return Result<Breed>.Success(breed);
        }
    }
}