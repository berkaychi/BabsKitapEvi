using Microsoft.AspNetCore.Identity;

namespace BabsKitapEvi.Entities.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<IdentityUserRole<string>>? UserRoles { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}