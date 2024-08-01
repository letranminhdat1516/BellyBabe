using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Model.VerifyPhoneNumber;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;
        private readonly ILogger _logger;
        private readonly UserService _userService;

        public OtpController(UserService userService, OtpService otpService, ILogger<OtpController> logger)
        {
            _userService = userService;
            _otpService = otpService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpPost("request")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.PhoneNumber))
            {
                return BadRequest(new { message = "Model hoặc số điện thoại không hợp lệ." });
            }

            var phoneNumberExists = await _userService.CheckPhoneNumberExistsAsync(model.PhoneNumber);

            if (!phoneNumberExists)
            {
                _logger.LogWarning($"Số điện thoại {model.PhoneNumber} chưa được đăng ký.");
                return NotFound(new { message = "Số điện thoại này chưa được đăng ký." });
            }

            var otp = _otpService.GenerateOtp();
            await _otpService.SaveOtpAsync(model.PhoneNumber, otp);
            await _otpService.SendOtpViaSmsAsync(model.PhoneNumber, otp);

            _logger.LogInformation($"OTP đã được gửi thành công tới số điện thoại {model.PhoneNumber}.");
            return Ok(new { phoneNumber = model.PhoneNumber, message = "OTP đã được gửi thành công." });
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
