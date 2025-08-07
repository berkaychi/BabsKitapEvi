using BabsKitapEvi.Entities.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BabsKitapEvi.Entities.DTOs.AuthDTOs
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