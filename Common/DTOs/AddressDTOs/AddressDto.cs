namespace BabsKitapEvi.Common.DTOs.AddressDTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}