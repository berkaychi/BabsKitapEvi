using BabsKitapEvi.Common.DTOs.OrderDTOs;

namespace BabsKitapEvi.Common.DTOs.OrderDTOs
{
    public sealed class UserOrdersDto
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime LatestOrderDate { get; set; }
        public List<OrderDto> Orders { get; set; } = new();
    }
}