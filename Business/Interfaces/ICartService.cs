using BabsKitapEvi.Entities.DTOs.CartDTOs;
using System.Threading.Tasks;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICartService
    {
        Task<Result<CartDto>> GetCartByUserIdAsync(string userId);
        Task<Result<string>> AddItemToCartAsync(string userId, AddCartItemDto itemDto);
        Task<Result<string>> RemoveItemFromCartAsync(string userId, int bookId);
        Task<Result<string>> UpdateItemInCartAsync(string userId, int bookId, UpdateCartItemDto itemDto);
        Task<Result<string>> ClearCartAsync(string userId);
    }
}