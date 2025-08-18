using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BabsKitapEvi.Business.Services
{
    public sealed class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        public UserManager(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<Result<string>> ChangePasswordAsync(string userId, UserForChangePasswordDto userForChangePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.Failure(404, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, userForChangePasswordDto.OldPassword, userForChangePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(400, result.Errors.Select(e => e.Description).ToList());
            }
            return Result<string>.Succeed("Password changed successfully.");
        }

        public async Task<Result<string>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.Failure(404, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(400, result.Errors.Select(e => e.Description).ToList());
            }
            return Result<string>.Succeed("User deleted successfully.");
        }

        public async Task<Result<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userDtos = _mapper.Map<List<UserResponseDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                userDtos[i].Role = roles.FirstOrDefault() ?? "User";
            }


            return Result<IEnumerable<UserResponseDto>>.Succeed(userDtos);
        }

        public async Task<Result<UserResponseDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserResponseDto>.Failure(404, "User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserResponseDto>(user);
            userDto.Role = roles.FirstOrDefault() ?? "User";

            return Result<UserResponseDto>.Succeed(userDto);
        }

        public async Task<Result<string>> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.Failure(404, "User not found.");
            }

            _mapper.Map(userForUpdateDto, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(400, result.Errors.Select(e => e.Description).ToList());
            }
            return Result<string>.Succeed("User updated successfully.");
        }

    }
}