namespace BabsKitapEvi.Common.DTOs.OrderDTOs
{
    public sealed class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;

        public string ShippingFullName { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingCountry { get; set; } = null!;
        public string ShippingZipCode { get; set; } = null!;

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}