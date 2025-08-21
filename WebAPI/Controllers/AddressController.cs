using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.AddressDTOs;
using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    public sealed class AddressController : PrivateBaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var result = await _addressService.GetAddressesByUserIdAsync(UserId);
            return CreateActionResult(result);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _addressService.GetAddressByIdAsync(id, UserId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto createAddressDto)
        {
            var userAddressResult = await _addressService.GetAddressesByUserIdAsync(UserId);
            if (userAddressResult.IsSuccess && !userAddressResult.Data.Any())
            {
                createAddressDto.IsDefault = true;
            }
            var result = await _addressService.CreateAddressAsync(createAddressDto, UserId);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] CreateAddressDto updateAddressDto)
        {
            var result = await _addressService.UpdateAddressAsync(id, updateAddressDto, UserId);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddressAsync(id, UserId);
            return CreateActionResult(result);
        }

        [HttpPost("{id}/set-default")]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var result = await _addressService.SetDefaultAddressAsync(id, UserId);
            return CreateActionResult(result);
        }
    }
}