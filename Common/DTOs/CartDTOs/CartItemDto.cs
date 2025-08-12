namespace BabsKitapEvi.Common.DTOs.CartDTOs
{
    public sealed class CartItemDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}