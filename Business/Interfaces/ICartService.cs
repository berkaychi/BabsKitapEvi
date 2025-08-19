using BabsKitapEvi.Common.DTOs.CartDTOs;
using System.Threading.Tasks;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICartService
    {
        Task<IServiceResult> GetCartByUserIdAsync(string userId);
        Task<IServiceResult> AddItemToCartAsync(string userId, AddCartItemDto itemDto);
        Task<IServiceResult> RemoveItemFromCartAsync(string userId, int bookId);
        Task<IServiceResult> UpdateItemInCartAsync(string userId, int bookId, UpdateCartItemDto itemDto);
        Task<IServiceResult> ClearCartAsync(string userId);
    }
}