using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Swp391DbContext;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginController : ControllerBase
    {
        private readonly OtpService _otpService;
        
        public UserLoginController(OtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpModel model)
        {
            var otp = _otpService.GenerateOtp();
            await _otpService.SaveOtpAsync(model.PhoneNumber, otp, model.UserName);
            await _otpService.SendOtpViaSmsAsync(model.PhoneNumber, otp);
            return Ok(new { message = "OTP sent successfully." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyUserOtpModel model)
        {
            var isValid = await _otpService.VerifyOtpAsync(model.PhoneNumber, model.OTP);
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid OTP." });
            }

            // Generate and return token
            var tokenResponse = await _otpService.LoginAsync(new UserLoginDTO
            {
                PhoneNumber = model.PhoneNumber,
                OTP = model.OTP
            });

            return Ok(tokenResponse);
        }
        [HttpGet("check-otp")]
        public async Task<IActionResult> GetOtp([FromQuery] string phoneNumber)
        {
            var otp = await _otpService.GetOtpByPhoneNumberAsync(phoneNumber);
            if (otp == null)
            {
                return NotFound(new { message = "OTP not found." });
            }

            return Ok(new { otp });
        }
    }

    public record RequestOtpModel
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }

    public record VerifyUserOtpModel
    {
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
    }
}
