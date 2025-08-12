using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.AuthDTOs;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    public sealed class AuthController : CustomBaseController
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<AppUser>(userForRegisterDto);
            userToCreate.UserName = userForRegisterDto.Email;

            var result = await _authService.Register(userToCreate, userForRegisterDto.Password);

            return CreateActionResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var result = await _authService.Login(userForLoginDto.Email, userForLoginDto.Password);
            return CreateActionResult(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshTokenLoginAsync(refreshTokenDto);
            return CreateActionResult(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.LogoutAsync(refreshTokenDto);
            return CreateActionResult(result);
        }
    }
}