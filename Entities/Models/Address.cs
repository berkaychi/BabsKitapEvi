namespace BabsKitapEvi.Entities.Models
{
    public sealed class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public string AddressName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
    }
}