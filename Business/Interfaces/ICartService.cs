using BabsKitapEvi.Common.DTOs.CartDTOs;
using System.Threading.Tasks;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICartService
    {
        Task<IServiceResult<CartDto>> GetCartByUserIdAsync(string userId);
        Task<IServiceResult<CartDto>> AddItemToCartAsync(string userId, AddCartItemDto itemDto);
        Task<IServiceResult<CartDto>> RemoveItemFromCartAsync(string userId, int bookId);
        Task<IServiceResult<CartDto>> UpdateItemInCartAsync(string userId, int bookId, UpdateCartItemDto itemDto);
        Task<IServiceResult> ClearCartAsync(string userId);
    }
}