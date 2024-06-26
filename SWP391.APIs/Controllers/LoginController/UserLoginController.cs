using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Model.Login;
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
    }

<<<<<<< HEAD
    public class RequestOtpModel
=======
    public record RequestOtpModel
>>>>>>> master
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }

<<<<<<< HEAD
    public class VerifyUserOtpModel
=======
    public record VerifyUserOtpModel
>>>>>>> master
    {
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
    }
}
