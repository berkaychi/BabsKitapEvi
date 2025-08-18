using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BabsKitapEvi.Entities.Models
{
    public class AppRole : IdentityRole
    {
        public virtual ICollection<IdentityUserRole<string>>? UserRoles { get; set; }
    }
}