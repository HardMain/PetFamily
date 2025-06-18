using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species
{
    public class Species
    {
        private readonly List<Breed> _breeds = [];

        private Species(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Species>("Name can not be empty!");

            return Result.Success(new Species(name));
        }
    }
}
