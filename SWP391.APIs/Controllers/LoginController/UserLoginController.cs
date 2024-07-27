using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.BLL.Services;
using SWP391.BLL.Services.LoginService;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Swp391DbContext;
using System.Threading.Tasks;
namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly OtpService _otpService;
        private readonly AuthService _authService;
        public UserLoginController(UserService userService, OtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.PhoneNumber))
            {
                return BadRequest(new { message = "Phone number is required." });
            }

            var userResponse = await _userService.UserLoginAsync(model);
            if (userResponse == null)
            {
                return Unauthorized(new { message = "Invalid phone number or password." });
            }

          

            return Ok(userResponse);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            try
            {
                var newUser = await _userService.RegisterAsync(model);
                return Ok(new { message = "Registration successful", user = newUser });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("complete-first-login")]
        public async Task<IActionResult> CompleteFirstLogin([FromBody] FirstTimeUserInfoModel model)
        {
            var user = await _userService.CompleteFirstLoginAsync(model);
            if (user == null)
            {
                return BadRequest(new { message = "Completion failed." });
            }

            return Ok(new { message = "User information updated successfully." });
        }
    }
}