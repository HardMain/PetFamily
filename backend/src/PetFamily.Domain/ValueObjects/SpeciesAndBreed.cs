using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    internal class SpeciesAndBreed
    {
        private SpeciesAndBreed(Guid speciesId, Guid breedId) 
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public Guid SpeciesId { get; }
        public Guid BreedId { get; }

        public Result<SpeciesAndBreed> Create(Guid speciesId, Guid breedId)
        {
            return Result.Success(new SpeciesAndBreed(speciesId, breedId));
        }
    }
}
