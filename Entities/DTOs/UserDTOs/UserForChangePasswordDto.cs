namespace BabsKitapEvi.Entities.DTOs.UserDTOs
{
    public sealed class UserForChangePasswordDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}