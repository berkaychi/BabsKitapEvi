namespace BabsKitapEvi.Entities.DTOs.CartDTOs
{
    public sealed class AddCartItemDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}