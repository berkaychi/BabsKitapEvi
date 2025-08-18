using BabsKitapEvi.Common.DTOs.AddressDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IAddressService
    {
        Task<IServiceResult> GetAddressesByUserIdAsync(string userId);
        Task<IServiceResult> GetAddressByIdAsync(int addressId, string userId);
        Task<IServiceResult> CreateAddressAsync(CreateAddressDto createAddressDto, string userId);
        Task<IServiceResult> UpdateAddressAsync(int addressId, CreateAddressDto updateAddressDto, string userId);
        Task<IServiceResult> DeleteAddressAsync(int addressId, string userId);
        Task<IServiceResult> SetDefaultAddressAsync(int addressId, string userId);
    }
}