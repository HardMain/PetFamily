using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species
{
    internal class Species
    {
        private readonly List<Breed> _breeds = [];

        private Species(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public IReadOnlyList<Breed> Breeds => _breeds;

        public Result<Species> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Species>("Name can not be empty!");

            return Result.Success(new Species(name));
        }
    }
}
