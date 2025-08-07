using BabsKitapEvi.Entities.DTOs.AuthDTOs;
using BabsKitapEvi.Entities.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserDto>> Register(AppUser user, string password);
        Task<Result<AuthResponseDto>> Login(string email, string password);
        Task<Result<TokenDto>> RefreshTokenLoginAsync(RefreshTokenDto refreshTokenDto);
        Task<Result<string>> LogoutAsync(RefreshTokenDto refreshTokenDto);
    }
}