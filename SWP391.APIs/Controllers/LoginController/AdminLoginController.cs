using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.LoginService;
using SWP391.DAL.Model.Login;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers.LoginController
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminLoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AdminLoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Invalid client request");
            }

            var response = await _authService.AdminLoginAsync(loginDTO);
            if (response == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _authService.GenerateJwtToken(response.Email, "Admin", response.UserID);

            // Assuming `response` contains the user information
            return Ok(new
            {
                Token = token,
                response.UserID,
                response.UserName,
                response.PhoneNumber,
                response.Password,
                response.Email,
                response.Address,
                response.FullName,
                response.RoleId,
                response.Image,
                response.IsFirstLogin
            });
        }
    }
}
