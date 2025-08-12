using System.Text.Json.Serialization;
using BabsKitapEvi.Common.DTOs.UserDTOs;

namespace BabsKitapEvi.Common.DTOs.AuthDTOs
{
    public sealed class AuthResponseDto
    {
        [JsonPropertyName("user")]
        public UserResponseDto User { get; set; } = null!;

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = null!;

        [JsonPropertyName("expiresIn")]
        public long ExpiresIn { get; set; }
    }
}