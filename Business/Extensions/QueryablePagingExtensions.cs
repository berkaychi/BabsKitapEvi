using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.EntityFrameworkCore;

namespace BabsKitapEvi.Business.Extensions
{
    public static class QueryablePagingExtensions
    {
        public static async Task<PageResult<T>> ToPageResultAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResult<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}