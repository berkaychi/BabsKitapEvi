using BabsKitapEvi.Common.DTOs.AddressDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IAddressService
    {
        Task<IServiceResult<IEnumerable<AddressDto>>> GetAddressesByUserIdAsync(string userId);
        Task<IServiceResult<AddressDto>> GetAddressByIdAsync(int addressId, string userId);
        Task<IServiceResult<AddressDto>> CreateAddressAsync(CreateAddressDto createAddressDto, string userId);
        Task<IServiceResult<AddressDto>> UpdateAddressAsync(int addressId, CreateAddressDto updateAddressDto, string userId);
        Task<IServiceResult> DeleteAddressAsync(int addressId, string userId);
        Task<IServiceResult<AddressDto>> SetDefaultAddressAsync(int addressId, string userId);
    }
}