using System.IO.Compression;
using System.Security;
using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.AddressDTOs;
using BabsKitapEvi.Common.Results;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BabsKitapEvi.Business.Services
{
    public sealed class AddressManager : IAddressService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddressManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IServiceResult<AddressDto>> CreateAddressAsync(CreateAddressDto createAddressDto, string userId)
        {
            var address = _mapper.Map<Address>(createAddressDto);
            address.UserId = userId;

            _context.Add(address);
            await _context.SaveChangesAsync();

            var addressDto = _mapper.Map<AddressDto>(address);
            return new SuccessDataResult<AddressDto>(addressDto, 201, "Address created successfully.");
        }

        public async Task<IServiceResult> DeleteAddressAsync(int addressId, string userId)
        {
            var address = await GetAddressIfExists(addressId, userId);

            if (address == null)
            {
                return new ErrorResult(404, "Address not found.");
            }

            if (address.IsDefault)
            {
                var otherAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.Id != addressId);

                if (otherAddress != null)
                {
                    otherAddress.IsDefault = true;
                }
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return new SuccessResult(204, "Address deleted successfully.");
        }

        public async Task<IServiceResult<AddressDto>> GetAddressByIdAsync(int addressId, string userId)
        {
            var address = await GetAddressIfExists(addressId, userId);

            if (address == null)
            {
                return new ErrorDataResult<AddressDto>(default!, 404, "Address not found.");
            }

            var addressDto = _mapper.Map<AddressDto>(address);
            return new SuccessDataResult<AddressDto>(addressDto, 200, "Address retrieved successfully.");
        }

        public async Task<IServiceResult<IEnumerable<AddressDto>>> GetAddressesByUserIdAsync(string userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.AddressName)
                .ToListAsync();

            var addressDtos = _mapper.Map<IEnumerable<AddressDto>>(addresses);
            return new SuccessDataResult<IEnumerable<AddressDto>>(addressDtos, 200, "Addresses retrieved successfully.");
        }

        public async Task<IServiceResult<AddressDto>> SetDefaultAddressAsync(int addressId, string userId)
        {
            var address = await GetAddressIfExists(addressId, userId);
            if (address == null)
            {
                return new ErrorDataResult<AddressDto>(default!, 404, "Address not found.");
            }

            var userAddresses = await _context.Addresses
               .Where(a => a.UserId == userId)
               .ToListAsync();

            foreach (var userAddress in userAddresses)
            {
                userAddress.IsDefault = userAddress.Id == addressId;
            }

            await _context.SaveChangesAsync();
            return new SuccessDataResult<AddressDto>(default!, 200, "Default address updated successfully.");
        }

        public async Task<IServiceResult<AddressDto>> UpdateAddressAsync(int addressId, CreateAddressDto updateAddressDto, string userId)
        {
            var address = await GetAddressIfExists(addressId, userId);
            if (address == null)
            {
                return new ErrorDataResult<AddressDto>(default!, 404, "Address not found.");
            }

            _mapper.Map(updateAddressDto, address);
            await _context.SaveChangesAsync();

            var addressDto = _mapper.Map<AddressDto>(address);
            return new SuccessDataResult<AddressDto>(addressDto, 200, "Address updated successfully.");
        }

        private async Task<Address?> GetAddressIfExists(int addressId, string userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        }
    }
}