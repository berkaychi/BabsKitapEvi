using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BabsKitapEvi.Common.Results;

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

        public async Task<IServiceResult> ChangePasswordAsync(string userId, UserForChangePasswordDto userForChangePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ErrorResult(404, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, userForChangePasswordDto.OldPassword, userForChangePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ErrorResult(400, "Password change failed.", errors);
            }
            return new SuccessResult(200, "Password changed successfully.");
        }

        public async Task<IServiceResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ErrorResult(404, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ErrorResult(400, "User deletion failed.", errors);
            }
            return new SuccessResult(200, "User deleted successfully.");
        }

        public async Task<IServiceResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userDtos = _mapper.Map<List<UserResponseDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                userDtos[i].Role = roles.FirstOrDefault() ?? "User";
            }

            return new SuccessDataResult<IEnumerable<UserResponseDto>>(userDtos, 200, "Users retrieved successfully.");
        }

        public async Task<IServiceResult<UserResponseDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ErrorDataResult<UserResponseDto>(default!, 404, "User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserResponseDto>(user);
            userDto.Role = roles.FirstOrDefault() ?? "User";

            return new SuccessDataResult<UserResponseDto>(userDto, 200, "User retrieved successfully.");
        }

        public async Task<IServiceResult<UserResponseDto>> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ErrorDataResult<UserResponseDto>(default!, 404, "User not found.");
            }

            _mapper.Map(userForUpdateDto, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ErrorDataResult<UserResponseDto>(default!, 400, errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var updatedUserDto = _mapper.Map<UserResponseDto>(user);
            updatedUserDto.Role = roles.FirstOrDefault() ?? "User";
            return new SuccessDataResult<UserResponseDto>(updatedUserDto, 200, "User updated successfully.");
        }

        public async Task<IServiceResult<UserResponseDto>> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ErrorDataResult<UserResponseDto>(default!, 404, "User not found.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                return new ErrorDataResult<UserResponseDto>(default!, 400, $"Role '{newRole}' does not exist.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = removeResult.Errors.Select(e => e.Description).ToList();
                    return new ErrorDataResult<UserResponseDto>(default!, 400, errors);
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                var errors = addResult.Errors.Select(e => e.Description).ToList();
                return new ErrorDataResult<UserResponseDto>(default!, 400, errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var updatedUserDto = _mapper.Map<UserResponseDto>(user);
            updatedUserDto.Role = roles.FirstOrDefault() ?? "User";
            return new SuccessDataResult<UserResponseDto>(updatedUserDto, 200, "User role updated successfully.");
        }

    }
}