using System.IO.Compression;
using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.Results;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Enums;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IServiceResult<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult<OrderDto>> GetOrderByIdAsync(int orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult<IEnumerable<OrderDto>>> GetOrdersForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}