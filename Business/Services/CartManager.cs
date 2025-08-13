using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Common.DTOs.CartDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TS.Result;

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

        public async Task<Result<string>> AddItemToCartAsync(string userId, AddCartItemDto itemDto)
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
                    return Result<string>.Failure(404, "Book not found.");
                }
            }
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Item added to cart.");
        }

        public async Task<Result<CartDto>> GetCartByUserIdAsync(string userId)
        {

            var cart = await GetOrCreateCartAsync(userId);

            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.TotalPrice = cart.Items.Sum(item => item.Book.Price * item.Quantity);
            return Result<CartDto>.Succeed(cartDto);
        }

        public async Task<Result<string>> RemoveItemFromCartAsync(string userId, int bookId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return Result<string>.Succeed("Item removed from cart.");
            }
            return Result<string>.Failure(404, "Item not found in cart.");
        }

        public async Task<Result<string>> ClearCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Cart cleared.");
        }

        public async Task<Result<string>> UpdateItemInCartAsync(string userId, int bookId, UpdateCartItemDto itemDto)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (cartItem == null)
            {
                return Result<string>.Failure(404, "Cart item not found.");
            }

            if (itemDto.Quantity > 0)
            {
                cartItem.Quantity = itemDto.Quantity;
            }
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Cart item updated.");
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