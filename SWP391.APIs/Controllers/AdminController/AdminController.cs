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

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            if (user == null)
            {
                return BadRequest("User creation failed.");
            }
            return Ok(user);
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
        [HttpPost("send-voucher-to-users")]
        public async Task<IActionResult> SendVoucherToUsers([FromBody] SendVoucherRequest request)
        {
            var result = await _userService.SendVoucherToUsersAsync(request.UserIds, request.VoucherCode);
            if (!result)
            {
                return BadRequest("Voucher not sent or no users found.");
            }

            return Ok("Vouchers sent to specified users.");
        }
    }
}
