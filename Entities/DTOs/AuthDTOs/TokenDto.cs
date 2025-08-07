using System.Text.Json.Serialization;

namespace BabsKitapEvi.Entities.DTOs.AuthDTOs
{
    public sealed class TokenDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = null!;
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = null!;
    }
}