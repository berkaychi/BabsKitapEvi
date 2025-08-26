using BabsKitapEvi.Entities.Enums;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IStockService
    {
        Task<StockStatus> GetStockStatusAsync(int bookId);
        Task<bool> HasAvailableStockAsync(int bookId, int quantity);
        Task<bool> ReduceStockAsync(int bookId, int quantity);
    }
}