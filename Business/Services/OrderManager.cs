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
            var cartResult = await _cartService.GetCartByUserIdAsync(userId);
            if (!cartResult.IsSuccess || cartResult.Data.Items.Count == 0)
            {
                return new ErrorDataResult<OrderDto>(default!, 400, "Cart is empty.");
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == createOrderDto.AddressId && a.UserId == userId);

            if (address == null)
            {
                return new ErrorDataResult<OrderDto>(default!, 404, "Address not found.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    TotalAmount = cartResult.Data.TotalPrice,
                    Status = OrderStatus.Pending,
                    ShippingFullName = address.FullName,
                    ShippingAddress = address.StreetAddress,
                    City = address.City,
                    Country = address.Country,
                    ZipCode = address.ZipCode
                };

                foreach (var cartItem in cartResult.Data.Items)
                {
                    var orderItem = new OrderItem
                    {
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price
                    };
                    order.OrderItems.Add(orderItem);
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await _cartService.ClearCartAsync(userId);

                await transaction.CommitAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return new SuccessDataResult<OrderDto>(orderDto, 201, "Order created successfully.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ErrorDataResult<OrderDto>(default!, 500, "Failed to create order. Please try again.");
            }
        }

        public async Task<IServiceResult<OrderDto>> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return new ErrorDataResult<OrderDto>(default!, 404, "Order not found.");
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return new SuccessDataResult<OrderDto>(orderDto, 200, "Order retrieved successfully.");
        }

        public async Task<IServiceResult<IEnumerable<OrderDto>>> GetOrdersForUserAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return new SuccessDataResult<IEnumerable<OrderDto>>(orderDtos, 200, "Orders retrieved successfully.");
        }

        public async Task<IServiceResult<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateOrderStatusDto, string userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return new ErrorDataResult<OrderDto>(default!, 404, "Order not found.");
            }

            order.Status = Enum.Parse<OrderStatus>(updateOrderStatusDto.Status);
            await _context.SaveChangesAsync();

            var orderDto = _mapper.Map<OrderDto>(order);
            return new SuccessDataResult<OrderDto>(orderDto, 200, "Order status updated successfully.");
        }

        public async Task<IServiceResult<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return new SuccessDataResult<IEnumerable<OrderDto>>(orderDtos, 200, "All orders retrieved successfully.");
        }

        public async Task<IServiceResult<IEnumerable<OrderDto>>> GetOrdersByIdAsync(int orderId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Where(o => o.Id == orderId)
                .ToListAsync();

            if (orders.Count == 0)
            {
                return new ErrorDataResult<IEnumerable<OrderDto>>(default!, 404, "No orders found with the given ID.");
            }

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return new SuccessDataResult<IEnumerable<OrderDto>>(orderDtos, 200, "Orders retrieved successfully.");
        }

        public async Task<IServiceResult<IEnumerable<UserOrdersDto>>> GetAllOrdersGroupedByUserAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Include(o => o.User)
                .ToListAsync();

            var groupedOrders = orders
                .GroupBy(o => o.UserId)
                .Select(g =>
                {
                    var user = g.First().User;
                    var userOrders = g.OrderByDescending(o => o.OrderDate).ToList();
                    var latestOrderDate = userOrders.First().OrderDate;

                    return new UserOrdersDto
                    {
                        UserId = g.Key,
                        FirstName = user?.FirstName ?? "",
                        LastName = user?.LastName ?? "",
                        UserEmail = user?.Email ?? "",
                        LatestOrderDate = latestOrderDate,
                        Orders = _mapper.Map<List<OrderDto>>(userOrders)
                    };
                })
                .OrderByDescending(u => u.LatestOrderDate)
                .ToList();

            return new SuccessDataResult<IEnumerable<UserOrdersDto>>(groupedOrders, 200, "Orders grouped by user retrieved successfully.");
        }
    }
}