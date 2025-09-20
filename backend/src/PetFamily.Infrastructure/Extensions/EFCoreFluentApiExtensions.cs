using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions
{
    public static class EFCoreFluentApiExtensions
    {
        public static PropertyBuilder<IReadOnlyList<TValueObject>> ValueObjectsCollectionJsonConverter<TValueObject, TDto>(
            this PropertyBuilder<IReadOnlyList<TValueObject>> builder,
            Func<TValueObject, TDto> toDtoSelector,
            Func<TDto, TValueObject> toValueObjectSelector)
        {
            return builder.HasConversion(
                valueObjects => SerializeValueObjectCollection(valueObjects, toDtoSelector),
                json => DeserializeDtoCollection(json, toValueObjectSelector),
                CreateCollectionValueComparer<TValueObject>())
                .HasColumnType("jsonb");
        }

        public static string SerializeValueObjectCollection<TValueObject, TDto>(
            IReadOnlyList<TValueObject> valueObjects,
            Func<TValueObject, TDto> toDtoSelector)
        {
            var dtos = valueObjects.Select(toDtoSelector);

            return JsonSerializer.Serialize(dtos, JsonSerializerOptions.Default);
        }

        public static IReadOnlyList<TValueObject> DeserializeDtoCollection<TValueObject, TDto>(
            string json,
            Func<TDto, TValueObject> toValueObjectSelector)
        {
            var dtos = JsonSerializer.Deserialize<IEnumerable<TDto>>(json, JsonSerializerOptions.Default) ?? [];

            return dtos.Select(toValueObjectSelector).ToList();
        }

        public static ValueComparer<IReadOnlyList<TValueObject>> CreateCollectionValueComparer<TValueObject>()
        {
            return new (
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList());
        }
    }
}
