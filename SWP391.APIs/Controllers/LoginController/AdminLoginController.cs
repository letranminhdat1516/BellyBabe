using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Model.Login;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers.LoginController
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly OtpService _otpService;

        public AdminLoginController(IAuthService authService, IEmailService emailService, OtpService otpService)
        {
            _authService = authService;
            _emailService = emailService;
            _otpService = otpService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Invalid client request");
            }

            var response = await _authService.AdminLoginAsync(loginDTO);
            if (response == null)
            {
                return Unauthorized();
            }

            // Generate and send OTP via email
            var otp = _otpService.GenerateOtp();
            await _otpService.SaveOtpAsync(loginDTO.Email, otp, loginDTO.Email, true);
            await _emailService.SendEmailAsync(loginDTO.Email, "Your OTP Code", $"Your OTP code is {otp}");

            return Ok(new { Message = "OTP sent to your email." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyAdminOtpModel verifyOtpModel)
        {
            if (verifyOtpModel == null)
            {
                return BadRequest("Invalid client request");
            }

            var isValidOtp = await _otpService.VerifyOtpAsync(verifyOtpModel.Email, verifyOtpModel.Otp, true);
            if (!isValidOtp)
            {
                return Unauthorized("Invalid OTP");
            }

            var token = _authService.GenerateJwtToken(verifyOtpModel.Email, "Admin");
            return Ok(new { Token = token });
        }
    }
}
