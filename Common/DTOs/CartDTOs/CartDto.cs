using System.Collections.Generic;

namespace BabsKitapEvi.Common.DTOs.CartDTOs
{
    public sealed class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}