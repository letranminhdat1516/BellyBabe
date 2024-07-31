using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Model.users;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using SWP391.DAL.Model.Voucher;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreateDTO userDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    ErrorCode = "USER_CREATION_ERROR",
                    Status = 400
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Đã xảy ra lỗi khi tạo người dùng.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Status = 500
                });
            }
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDTO userDto)
        {
            var user = await _userService.UpdateUserAsync(userId, userDto);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }


        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                if (!result)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(new { message = "User deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Cannot delete admin users.")
                {
                    return BadRequest(new { message = "Cannot delete admin users." });
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto, OtpService otpService)
        {
            var otp = await _userService.GeneratePasswordResetTokenAsync(forgotPasswordDto.Email, otpService: otpService);
            if (otp == null)
            {
                return BadRequest("Email not found.");
            }
            return Ok(new { otp = otp });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Otp, resetPasswordDto.NewPassword);
            if (!result)
            {
                return BadRequest("Invalid OTP or OTP expired.");
            }
            return Ok("Password reset successful.");
        }
        [HttpPost("ban-user/{userId}")]
        public async Task<IActionResult> BanUser(int userId)
        {
            try
            {
                var result = await _userService.BanUserAsync(userId);
                if (!result)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(new { message = "User banned successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
