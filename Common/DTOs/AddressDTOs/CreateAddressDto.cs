using System.ComponentModel.DataAnnotations;

namespace BabsKitapEvi.Common.DTOs.AddressDTOs
{
    public class CreateAddressDto
    {
        [Required]
        public string AddressName { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string StreetAddress { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}