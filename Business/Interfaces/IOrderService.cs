using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IOrderService
    {
        Task<IServiceResult> CreateOrderAsync(CreateOrderDto createOrderDto, string userId);
        Task<IServiceResult> GetOrdersForUserAsync(string userId);
        Task<IServiceResult> GetOrderByIdAsync(int orderId, string userId);
    }
}