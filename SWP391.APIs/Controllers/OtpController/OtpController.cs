using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Model.VerifyPhoneNumber;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;

        public OtpController(OtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpModel model)
        {
            var otp = _otpService.GenerateOtp();
            await _otpService.SaveOtpAsync(model.PhoneNumber, otp, "GuestUser");
            await _otpService.SendOtpViaSmsAsync(model.PhoneNumber, otp);
            return Ok(new { phoneNumber = model.PhoneNumber, message = "OTP sent successfully." });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpModel model)
        {
            var isValid = await _otpService.VerifyOtpAsync(model.PhoneNumber, model.OTP);
            if (!isValid)
            {
                return BadRequest(new { message = "Invalid OTP." });
            }

            var user = await _otpService.GetUserByPhoneNumberAsync(model.PhoneNumber);
            if (user != null)
            {
                user.IsActive = true;
                await _otpService.UpdateUserAsync(user);
            }

            return Ok(new { message = "Phone number verified successfully." });
        }

        [HttpGet("getOtp")]
        public async Task<IActionResult> GetOtp([FromQuery] string phoneNumber)
        {
            var otp = await _otpService.GetOtpByPhoneNumberAsync(phoneNumber);
            if (otp == null)
            {
                return NotFound(new { message = "OTP not found." });
            }

            return Ok(new { phoneNumber = phoneNumber, otp = otp });
        }

        public class RequestOtpModel
        {
            public string PhoneNumber { get; set; }
        }

        public class VerifyOtpModel
        {
            public string PhoneNumber { get; set; }
            public string OTP { get; set; }
        }
    }
}
