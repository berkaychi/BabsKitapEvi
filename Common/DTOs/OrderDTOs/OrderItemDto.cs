namespace BabsKitapEvi.Common.DTOs.OrderDTOs
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}