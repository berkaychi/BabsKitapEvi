using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.CartDTOs;
using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class CartsController : PrivateBaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyCart()
        {
            var result = await _cartService.GetCartByUserIdAsync(UserId);
            return CreateActionResult(result);
        }


        [HttpPost("me/items")]
        public async Task<IActionResult> AddItemToMyCart([FromBody] AddCartItemDto itemDto)
        {
            var result = await _cartService.AddItemToCartAsync(UserId, itemDto);
            return CreateActionResult(result);
        }

        [HttpPut("me/items/{bookId}")]
        public async Task<IActionResult> UpdateItemInMyCart(int bookId, [FromBody] UpdateCartItemDto itemDto)
        {
            var result = await _cartService.UpdateItemInCartAsync(UserId, bookId, itemDto);
            return CreateActionResult(result);
        }

        [HttpDelete("me/items/{bookId}")]
        public async Task<IActionResult> RemoveItemFromMyCart(int bookId)
        {
            var result = await _cartService.RemoveItemFromCartAsync(UserId, bookId);
            return CreateActionResult(result);
        }

        [HttpDelete("me/items/clear")]
        public async Task<IActionResult> ClearMyCart()
        {
            var result = await _cartService.ClearCartAsync(UserId);
            return CreateActionResult(result);
        }
    }
}