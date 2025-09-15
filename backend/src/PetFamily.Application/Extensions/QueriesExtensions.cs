using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;
using PetFamily.Contracts.DTOs.Volunteers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PetFamily.Application.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<PagedList<T>> ToPagedList<T>(
            this IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);

            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> SortByIf<T>(
            this IQueryable<T> source,
            bool condition,
            string sortBy,
            bool? Ask)
        {
            if (!condition)
                return source;

            var param = Expression.Parameter(typeof(T), "x");

            Expression body = param;

            foreach (var member in sortBy.Split('.'))
                body = Expression.Property(body, member);

            body = Expression.Convert(body, typeof(object));

            var lambda = Expression.Lambda<Func<T, object>>(body, param);

            return source = Ask is false
                ? source.OrderByDescending(lambda)
                : source.OrderBy(lambda);
        }
    }
}
