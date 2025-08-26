using System.Linq.Expressions;
using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Entities.Models;

namespace BabsKitapEvi.Business.Extensions
{
    public static class OrderQueryableExtensions
    {
        private static readonly Dictionary<string, Expression<Func<Order, object>>> SortableFields =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "OrderDate", o => o.OrderDate },
                { "TotalAmount", o => o.TotalAmount },
                { "Status", o => o.Status },
                { "UserId", o => o.UserId },
                { "ShippingFullName", o => o.ShippingFullName }
            };

        public static IQueryable<Order> ApplyFilters(this IQueryable<Order> query, OrdersQuery filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                var term = filters.SearchTerm.Trim();
                query = query.Where(o =>
                    o.ShippingFullName.Contains(term) ||
                    o.ShippingAddress.Contains(term) ||
                    o.City.Contains(term) ||
                    o.Country.Contains(term) ||
                    o.ZipCode.Contains(term) ||
                    o.OrderItems.Any(oi => oi.Book!.Title.Contains(term)) ||
                    (o.User != null && o.User.FirstName != null && o.User.LastName != null && (o.User.FirstName + " " + o.User.LastName).Contains(term)) ||
                    (o.User != null && o.User.FirstName != null && o.User.FirstName.Contains(term)) ||
                    (o.User != null && o.User.LastName != null && o.User.LastName.Contains(term)) ||
                    (o.User != null && o.User.Email != null && o.User.Email.Contains(term)));
            }

            if (filters.Status.HasValue)
            {
                query = query.Where(o => (int)o.Status == filters.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(filters.UserId))
            {
                query = query.Where(o => o.UserId == filters.UserId);
            }

            if (filters.MinAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= filters.MinAmount.Value);
            }

            if (filters.MaxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= filters.MaxAmount.Value);
            }

            if (filters.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filters.StartDate.Value);
            }

            if (filters.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= filters.EndDate.Value);
            }

            return query;
        }

        public static IQueryable<Order> ApplySorting(this IQueryable<Order> query,
            string? sortBy, string? sortDirection)
        {
            var normalizedSortBy = (sortBy ?? "OrderDate").Trim();
            var normalizedDirection = (sortDirection ?? "DESC").Trim().ToUpperInvariant();

            if (!SortableFields.TryGetValue(normalizedSortBy, out var sortExpr))
                sortExpr = SortableFields["OrderDate"];

            return normalizedDirection == "DESC"
                ? query.OrderByDescending(sortExpr)
                : query.OrderBy(sortExpr);
        }
    }
}