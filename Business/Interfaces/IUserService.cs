using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IUserService
    {
        Task<IServiceResult> GetAllUsersAsync();
        Task<IServiceResult> GetUserByIdAsync(string userId);
        Task<IServiceResult> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<IServiceResult> DeleteUserAsync(string userId);
        Task<IServiceResult> ChangePasswordAsync(string userId, UserForChangePasswordDto userForChangePasswordDto);
    }
}