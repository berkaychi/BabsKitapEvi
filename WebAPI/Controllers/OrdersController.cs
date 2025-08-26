using AutoMapper.Configuration.Annotations;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    public sealed class OrdersController : PrivateBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var result = await _orderService.CreateOrderAsync(createOrderDto, UserId);
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var isAdmin = User.IsInRole("Admin");
            var result = await _orderService.GetOrderByIdAsync(id, UserId, isAdmin);
            return CreateActionResult(result);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetOrdersForUser()
        {
            var result = await _orderService.GetOrdersForUserAsync(UserId);
            return CreateActionResult(result);
        }



        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto updateOrderStatusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, updateOrderStatusDto, UserId, true);
            return CreateActionResult(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return CreateActionResult(result);
        }

        [HttpGet("grouped-by-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrdersGroupedByUser()
        {
            var result = await _orderService.GetAllOrdersGroupedByUserAsync();
            return CreateActionResult(result);
        }

        [HttpGet("by-id/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersById(int id)
        {
            var result = await _orderService.GetOrdersByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] OrdersQuery query, CancellationToken ct)
        {
            var result = await _orderService.SearchOrdersAsync(query, ct);
            return CreateActionResult(result);
        }
    }
}