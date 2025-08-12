using System;
using System.Text.Json.Serialization;

namespace BabsKitapEvi.Common.DTOs.UserDTOs
{
    public sealed class UserResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = null!;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = null!;
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}