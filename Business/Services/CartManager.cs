using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Common.DTOs.CartDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Services
{
    public sealed class CartManager : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IServiceResult> AddItemToCartAsync(string userId, AddCartItemDto itemDto)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == itemDto.BookId);

            if (cartItem != null)
            {
                cartItem.Quantity += itemDto.Quantity;
            }
            else
            {
                var book = await _context.Books.FindAsync(itemDto.BookId);
                if (book != null)
                {
                    cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        BookId = itemDto.BookId,
                        Quantity = itemDto.Quantity
                    };
                    _context.CartItems.Add(cartItem);
                }
                else
                {
                    return new ErrorResult(404, "Book not found.");
                }
            }
            await _context.SaveChangesAsync();
            return new SuccessResult(200, "Item added to cart.");
        }

        public async Task<IServiceResult> GetCartByUserIdAsync(string userId)
        {

            var cart = await GetOrCreateCartAsync(userId);

            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.TotalPrice = cart.Items.Sum(item => item.Book.Price * item.Quantity);
            return new SuccessDataResult<CartDto>(cartDto, 200, "Cart retrieved successfully.");
        }

        public async Task<IServiceResult> RemoveItemFromCartAsync(string userId, int bookId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return new SuccessResult(200, "Item removed from cart.");
            }
            return new ErrorResult(404, "Item not found in cart.");
        }

        public async Task<IServiceResult> ClearCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return new SuccessResult(200, "Cart cleared.");
        }

        public async Task<IServiceResult> UpdateItemInCartAsync(string userId, int bookId, UpdateCartItemDto itemDto)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (cartItem == null)
            {
                return new ErrorResult(404, "Cart item not found.");
            }

            if (itemDto.Quantity > 0)
            {
                cartItem.Quantity = itemDto.Quantity;
            }
            await _context.SaveChangesAsync();
            return new SuccessResult(200, "Cart item updated.");
        }

        private async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _context.Carts
                 .Include(c => c.Items)
                 .ThenInclude(ci => ci.Book)
                 .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }
    }
}