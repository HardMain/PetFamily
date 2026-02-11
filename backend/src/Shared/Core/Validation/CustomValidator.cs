using FluentValidation;
using SharedKernel.Failures;

namespace Core.Validation
{
    public static class CustomValidator
    {
        public static IRuleBuilderOptionsConditions<T, TDto> MustBeValueObjects<T, TDto, TVo>(
            this IRuleBuilder<T, TDto> ruleBuilder,
            Func<TDto, Result<TVo>> VoFactory)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TVo> result = VoFactory(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
        }
        public static IRuleBuilderOptionsConditions<T, IEnumerable<TDto>> MustBeVoCollection<T, TDto, TVo, TListVo>(
            this IRuleBuilder<T, IEnumerable<TDto>> ruleBuilder,
            Func<TDto, Result<TVo>> elementFactory,
            Func<IEnumerable<TVo>, Result<TListVo>> listFactory)
        {
            return ruleBuilder.Custom((dtos, context) =>
            {
                var results = dtos.Select(elementFactory);

                var errors = results
                    .Where(r => r.IsFailure)
                    .Select(r => r.Error)
                    .ToList();

                foreach (var error in errors)
                    context.AddFailure(error.Serialize());

                if (errors.Any())
                    return;

                var listResult = listFactory(results.Select(r => r.Value));
                if (listResult.IsFailure)
                    context.AddFailure(listResult.Error.Serialize());
            });
        }

        public static IRuleBuilderOptions<T, TElement> WithError<T, TElement>(
            this IRuleBuilderOptions<T, TElement> ruleBuilder, Error error)
        {
            return ruleBuilder.WithMessage(error.Serialize());
        }
    }
}