using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TS.Result;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Authorize]
    public sealed class UsersController : CustomBaseController
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
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin" && currentUserId != id)
            {
                return CreateActionResult(Result<object>.Failure(403, "Forbidden"));
            }

            var result = await _userService.GetUserByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _userService.GetUserByIdAsync(userId);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            if (currentUserId != id && currentUserRole != "Admin")
            {
                return CreateActionResult(Result<object>.Failure(403, "Forbidden"));
            }

            var result = await _userService.UpdateUserAsync(id, userForUpdateDto);
            return CreateActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserForUpdateDto userForUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _userService.UpdateUserAsync(userId, userForUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }
            var result = await _userService.DeleteUserAsync(userId);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin" && currentUserId != id)
            {
                return CreateActionResult(Result<object>.Failure(403, "Forbidden"));
            }

            var result = await _userService.DeleteUserAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserForChangePasswordDto userForChangePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return CreateActionResult(Result<object>.Failure(401, "Unauthorized"));
            }

            var result = await _userService.ChangePasswordAsync(userId, userForChangePasswordDto);
            return CreateActionResult(result);
        }
    }
}