using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species
{
    internal class Breed
    {
        private Breed(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        public Result<Breed> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Breed>("Name can not be empty!");

            return Result.Success(new Breed(name));
        }
    }
}
