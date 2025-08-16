using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BPIBankSystem.API.Services;
using BPIBankSystem.API.Data;
using System.Security.Claims;
using BPIBankSystem.API.DTOs.Requests;

namespace BPIBankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result == "User registered successfully.")
            {
                return Ok(new CommonResponse<string>("success", result, null));
            }
            else
            {
                return BadRequest(new CommonResponse<string>("error", result, null));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (user, token, message) = await _authService.LoginAsync(dto);

            if (user == null || string.IsNullOrEmpty(token))
            {
                return BadRequest(new CommonResponse<object>
                {
                    status = "error",
                    message = message ?? "Đăng nhập thất bại.",
                    data = null
                });
            }

            return Ok(new CommonResponse<object>
            {
                status = "success",
                message = message,
                data = new
                {
                    user,
                    token
                }
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllAsync();
            return Ok(new CommonResponse<object>
            {
                status = "success",
                message = "Lấy danh sách người dùng thành công.",
                data = users
            });
        }

        [Authorize]
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto dto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");
            Console.WriteLine($"UpdateUser: userId={userId}, currentUserId={currentUserId}, isAdmin={isAdmin}");
            if (!isAdmin && currentUserId != userId)
            {
                return Unauthorized(new CommonResponse<string>("error", "Bạn không có quyền cập nhật thông tin của người dùng này.", null));
            }

            var result = await _authService.UpdateAsync(userId, dto);
            if (result.Contains("thành công"))
            {
                return Ok(new CommonResponse<string>("success", result, null));
            }
            else
            {
                return BadRequest(new CommonResponse<string>("error", result, null));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _authService.DeleteAsync(userId);
            if (result.Contains("thành công"))
            {
                return Ok(new CommonResponse<string>("success", result, null));
            }
            else
            {
                return BadRequest(new CommonResponse<string>("error", result, null));
            }
        }
    }

}