using System.ComponentModel.DataAnnotations;

namespace BabsKitapEvi.Entities.Models
{
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiryTime { get; set; }

        public AppUser User { get; set; } = null!;
    }
}