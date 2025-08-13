using BabsKitapEvi.Entities.Models;

namespace BabsKitapEvi.Business.DTOs.MappingDTOs
{
    public sealed class LoginInfoForMappingDto
    {
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiryTime { get; set; }
        public string RefreshToken { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}