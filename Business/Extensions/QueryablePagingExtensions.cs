using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.EntityFrameworkCore;

namespace BabsKitapEvi.Business.Extensions
{
    public static class QueryablePagingExtensions
    {
        public static async Task<PageResult<T>> ToPageResultAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var totalCount = await source.CountAsync(ct);
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PageResult<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}