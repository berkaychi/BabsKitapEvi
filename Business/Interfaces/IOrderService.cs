using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IOrderService
    {
        Task<IServiceResult<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, string userId);
        Task<IServiceResult<IEnumerable<OrderDto>>> GetOrdersForUserAsync(string userId);
        Task<IServiceResult<OrderDto>> GetOrderByIdAsync(int orderId, string userId);
        Task<IServiceResult<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateOrderStatusDto, string userId);
        Task<IServiceResult<IEnumerable<OrderDto>>> GetAllOrdersAsync();
        Task<IServiceResult<IEnumerable<OrderDto>>> GetOrdersByIdAsync(int orderId);
        Task<IServiceResult<IEnumerable<UserOrdersDto>>> GetAllOrdersGroupedByUserAsync();
    }
}