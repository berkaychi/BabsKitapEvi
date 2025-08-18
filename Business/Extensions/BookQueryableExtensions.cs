using System.Linq.Expressions;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Entities.Models;

namespace BabsKitapEvi.Business.Extensions
{
    public static class BookQueryableExtensions
    {
        private static readonly Dictionary<string, Expression<Func<Book, object>>> SortableFields =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "Title", b => b.Title },
                { "Price", b => b.Price },
                { "PublishedDate", b => b.PublishedDate },
                { "Author", b => b.Author }
            };

        public static IQueryable<Book> ApplyFilters(this IQueryable<Book> query, BooksQuery filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                var term = filters.SearchTerm.Trim();
                query = query.Where(b =>
                    b.Title.Contains(term) ||
                    b.Author.Contains(term));
                // || (b.Description != null && b.Description.Contains(term)));
            }

            if (filters.CategoryId.HasValue)
            {
                query = query.Where(b =>
                    b.BookCategories!.Any(bc => bc.CategoryId == filters.CategoryId));
            }

            if (filters.PublisherId.HasValue)
            {
                query = query.Where(b =>
                    b.BookPublishers!.Any(bp => bp.PublisherId == filters.PublisherId));
            }

            if (filters.MinPrice.HasValue)
                query = query.Where(b => b.Price >= filters.MinPrice.Value);

            if (filters.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= filters.MaxPrice.Value);

            return query;
        }

        public static IQueryable<Book> ApplySorting(this IQueryable<Book> query,
            string? sortBy, string? sortDirection)
        {
            var normalizedSortBy = (sortBy ?? "Title").Trim();
            var normalizedDirection = (sortDirection ?? "ASC").Trim().ToUpperInvariant();

            if (!SortableFields.TryGetValue(normalizedSortBy, out var sortExpr))
                sortExpr = SortableFields["Title"];

            return normalizedDirection == "DESC"
                ? query.OrderByDescending(sortExpr)
                : query.OrderBy(sortExpr);
        }
    }
}