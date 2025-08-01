using FluentValidation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Validation
{
    public static class CustomValidator
    { 
        public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObjects<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder, 
            Func<TElement, Result<TValueObject>> factoryMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
        }
        public static IRuleBuilderOptions<T, TElement> WithError<T, TElement>(
            this IRuleBuilderOptions<T, TElement> ruleBuilder, Error error)
        {
            return ruleBuilder.WithMessage(error.Serialize());
        }
    }
}