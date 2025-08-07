using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BabsKitapEvi.Entities.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public Cart Cart { get; set; }
    }
}