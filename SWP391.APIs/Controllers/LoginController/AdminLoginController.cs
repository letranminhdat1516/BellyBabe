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

            var token = _authService.GenerateJwtToken(loginDTO.Email, "Admin");
            return Ok(new { Token = token });
        }
    }
}
