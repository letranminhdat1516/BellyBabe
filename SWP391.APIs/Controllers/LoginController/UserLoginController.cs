using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.BLL.Services;
using SWP391.BLL.Services.LoginService;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Repositories.Contract;
using SWP391.DAL.Repositories.UserRepository;
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
        private readonly IAuthService _authService;
  
        public UserLoginController(UserService userService, OtpService otpService, IAuthService authService)
        {
            _userService = userService;
            _otpService = otpService;
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.PhoneNumber))
            {
                return BadRequest(new { message = "Số điện thoại là bắt buộc." });
            }

            var userResponse = await _userService.UserLoginAsync(model);
            if (userResponse == null)
            {
                return Unauthorized(new { message = "Sai số điện thoại hoặc mật khẩu." });
            }

          

            return Ok(userResponse);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            try
            {
                var existingUser = await _userService.GetUserByPhoneNumberAsync(model.PhoneNumber);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Người dùng đã tồn tại với số điện thoại này." });
                }
                var newUser = await _userService.RegisterAsync(model);
                return Ok(new { message = "Đăng ký thành công", user = newUser });
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
                return BadRequest(new { message = "Hoàn thành không thành công." });
            }

            return Ok(new { message = "Thông tin người dùng được cập nhật thành công." });
        }
    }
}