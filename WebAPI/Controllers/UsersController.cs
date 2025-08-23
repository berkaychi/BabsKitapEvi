using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class UsersController : PrivateBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var result = await _userService.GetUserByIdAsync(UserId);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            var result = await _userService.UpdateUserAsync(id, userForUpdateDto);
            return CreateActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserForUpdateDto userForUpdateDto)
        {
            var result = await _userService.UpdateUserAsync(UserId, userForUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            var result = await _userService.DeleteUserAsync(UserId);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return CreateActionResult(result);
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UserForRoleUpdateDto userForRoleUpdateDto)
        {
            var result = await _userService.UpdateUserRoleAsync(id, userForRoleUpdateDto.Role);
            return CreateActionResult(result);

        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserForChangePasswordDto userForChangePasswordDto)
        {
            var result = await _userService.ChangePasswordAsync(UserId, userForChangePasswordDto);
            return CreateActionResult(result);
        }
    }
}