using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Entities.DTOs.CartDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TS.Result;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class CartsController : CustomBaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyCart()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _cartService.GetCartByUserIdAsync(currentUserId);
            return CreateActionResult(result);
        }


        [HttpPost("me/items")]
        public async Task<IActionResult> AddItemToMyCart([FromBody] AddCartItemDto itemDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _cartService.AddItemToCartAsync(currentUserId, itemDto);
            return CreateActionResult(result);
        }

        [HttpPut("me/items/{bookId}")]
        public async Task<IActionResult> UpdateItemInMyCart(int bookId, [FromBody] UpdateCartItemDto itemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _cartService.UpdateItemInCartAsync(userId, bookId, itemDto);
            return CreateActionResult(result);
        }

        [HttpDelete("me/items/{bookId}")]
        public async Task<IActionResult> RemoveItemFromMyCart(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _cartService.RemoveItemFromCartAsync(userId, bookId);
            return CreateActionResult(result);
        }



        [HttpDelete("me/items/clear")]
        public async Task<IActionResult> ClearMyCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _cartService.ClearCartAsync(userId);
            return CreateActionResult(result);
        }
    }
}