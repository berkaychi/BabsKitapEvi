using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Enums;

namespace BabsKitapEvi.Business.Services
{
    public sealed class StockManager : IStockService
    {
        private readonly ApplicationDbContext _context;

        public StockManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockStatus> GetStockStatusAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return StockStatus.OutOfStock;

            return book.StockQuantity > 0 ? StockStatus.InStock : StockStatus.OutOfStock;
        }

        public async Task<bool> HasAvailableStockAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            return book != null && book.StockQuantity >= quantity;
        }

        public async Task<bool> ReduceStockAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.StockQuantity < quantity) return false;

            book.StockQuantity -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}