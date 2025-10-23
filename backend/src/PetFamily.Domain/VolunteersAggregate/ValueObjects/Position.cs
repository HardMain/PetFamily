using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{

    namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
    {
        public record Position
        {
            public static Position None = new(0);
            public static Position First = new(1);

            private Position(int value)
            {
                Value = value;
            }

            public int Value { get; }

            public static Result<Position> Create(int number)
            {
                if (number < 0)
                    return Errors.General.ValueIsInvalid("position");

                return new Position(number);
            }
        }
    }
}