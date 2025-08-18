using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.Results;
using BabsKitapEvi.DataAccess;

namespace BabsKitapEvi.Business.Services
{
    public sealed class OrderManager : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;

        public OrderManager(ApplicationDbContext context, IMapper mapper, ICartService cartService)
        {
            _context = context;
            _mapper = mapper;
            _cartService = cartService;
        }

        public async Task<IServiceResult> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
        {
            var cartResult = await _cartService.GetCartByUserIdAsync(userId);

            throw new NotImplementedException();
        }

        public Task<IServiceResult> GetOrderByIdAsync(int orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> GetOrdersForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}