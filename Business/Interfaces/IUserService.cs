using BabsKitapEvi.Entities.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<Result<UserResponseDto>> GetUserByIdAsync(string userId);
        Task<Result<string>> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<Result<string>> DeleteUserAsync(string userId);
        Task<Result<string>> ChangePasswordAsync(string userId, UserForChangePasswordDto userForChangePasswordDto);
        Task<Result<UserResponseDto>> GetCurrentUserProfileAsync(string userId);
        Task<Result<string>> UpdateCurrentUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<Result<string>> DeleteCurrentUserAsync(string userId);
    }
}