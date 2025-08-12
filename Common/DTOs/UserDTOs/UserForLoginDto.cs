using System.ComponentModel.DataAnnotations;

namespace BabsKitapEvi.Common.DTOs.UserDTOs
{
    public sealed class UserForLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}