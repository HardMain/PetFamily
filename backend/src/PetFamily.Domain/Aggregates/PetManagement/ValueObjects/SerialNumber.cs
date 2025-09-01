using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{

    namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
    {
        public record SerialNumber
        {
            public static SerialNumber None = new(0);
            public static SerialNumber First = new(1);

            private SerialNumber(int value)
            {
                Value = value;
            }

            public int Value { get; }

            public static Result<SerialNumber> Create(int number)
            {
                if (number < 0)
                    return Errors.General.ValueIsInvalid("serial number");

                return new SerialNumber(number);
            }
        }
    }
}