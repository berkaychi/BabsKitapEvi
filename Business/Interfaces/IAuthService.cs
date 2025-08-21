using BabsKitapEvi.Common.DTOs.AuthDTOs;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IAuthService
    {
        Task<IServiceResult<UserResponseDto>> Register(AppUser user, string password);
        Task<IServiceResult<AuthResponseDto>> Login(string email, string password);
        Task<IServiceResult<TokenDto>> RefreshTokenLoginAsync(RefreshTokenDto refreshTokenDto);
        Task<IServiceResult> LogoutAsync(RefreshTokenDto refreshTokenDto);
    }
}