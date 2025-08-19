using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Entities.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Common.DTOs.AuthDTOs;
using BabsKitapEvi.Business.DTOs.MappingDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Services
{
    public sealed class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DataAccess.ApplicationDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public AuthManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IMapper mapper, DataAccess.ApplicationDbContext context, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IServiceResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ErrorResult(404, "Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                return new ErrorResult(400, "Invalid email or password.");
            }

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            var userRefreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            await _context.UserRefreshTokens.AddAsync(userRefreshToken);
            await _context.SaveChangesAsync();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault() ?? Roles.User;

            var loginInfo = new LoginInfoForMappingDto
            {
                User = user,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Role = userRole,
                ExpiryTime = token.ValidTo
            };

            var authResponse = _mapper.Map<AuthResponseDto>(loginInfo);
            return new SuccessDataResult<AuthResponseDto>(authResponse, 200, "Login successful.");
        }

        public async Task<IServiceResult> Register(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ErrorResult(400, "Registration failed.", errors);
            }

            await _userManager.AddToRoleAsync(user, Roles.User);

            var userDto = _mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto, 201, "User registered successfully.");
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT Secret Key is not configured.")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<IServiceResult> RefreshTokenLoginAsync(RefreshTokenDto refreshTokenDto)
        {
            var userRefreshToken = await _context.UserRefreshTokens.FirstOrDefaultAsync(urt => urt.Token == refreshTokenDto.RefreshToken);

            if (userRefreshToken == null || userRefreshToken.ExpiryTime <= DateTime.UtcNow)
            {
                return new ErrorResult(400, "Invalid or expired refresh token.");
            }

            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId);
            if (user == null)
            {
                return new ErrorResult(404, "User associated with the refresh token not found.");
            }

            var token = await GenerateJwtToken(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var newRefreshToken = GenerateRefreshToken();

            userRefreshToken.Token = newRefreshToken;
            userRefreshToken.ExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.UserRefreshTokens.Update(userRefreshToken);
            await _context.SaveChangesAsync();

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
            return new SuccessDataResult<TokenDto>(tokenDto, 200, "Token refreshed successfully.");
        }

        public async Task<IServiceResult> LogoutAsync(RefreshTokenDto refreshTokenDto)
        {
            var userRefreshToken = await _context.UserRefreshTokens.FirstOrDefaultAsync(urt => urt.Token == refreshTokenDto.RefreshToken);
            if (userRefreshToken == null)
            {
                return new ErrorResult(404, "Refresh token not found.");
            }

            _context.UserRefreshTokens.Remove(userRefreshToken);
            await _context.SaveChangesAsync();
            return new SuccessResult(200, "Logout successful.");
        }
    }
}