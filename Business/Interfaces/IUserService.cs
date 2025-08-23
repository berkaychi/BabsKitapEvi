using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IUserService
    {
        Task<IServiceResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<IServiceResult<UserResponseDto>> GetUserByIdAsync(string userId);
        Task<IServiceResult<UserResponseDto>> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<IServiceResult<UserResponseDto>> UpdateUserRoleAsync(string userId, string newRole);
        Task<IServiceResult> DeleteUserAsync(string userId);
        Task<IServiceResult> ChangePasswordAsync(string userId, UserForChangePasswordDto userForChangePasswordDto);
    }
}