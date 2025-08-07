using System.Collections.Generic;

namespace BabsKitapEvi.Entities.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}