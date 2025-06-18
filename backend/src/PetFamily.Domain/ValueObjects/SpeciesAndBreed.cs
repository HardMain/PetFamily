using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public class SpeciesAndBreed : ValueObject
    {
        private SpeciesAndBreed(Guid speciesId, Guid breedId) 
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public Guid SpeciesId { get; }
        public Guid BreedId { get; }

        public static Result<SpeciesAndBreed> Create(Guid speciesId, Guid breedId)
        {
            return Result.Success(new SpeciesAndBreed(speciesId, breedId));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SpeciesId;
            yield return BreedId;
        }
    }
}
