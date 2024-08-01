using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AdminLoginController> _logger;

        public AdminLoginController(IAuthService authService, ILogger<AdminLoginController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AdminLoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                _logger.LogWarning("Invalid client request: loginDTO is null");
                return BadRequest("Invalid client request");
            }

            _logger.LogInformation("Admin login attempt with email: {Email}", loginDTO.Email);

            var response = await _authService.AdminLoginAsync(loginDTO);
            if (response == null)
            {
                _logger.LogWarning("Unauthorized login attempt for email: {Email}", loginDTO.Email);
                return Unauthorized("Invalid email or password");
            }

            var token = _authService.GenerateJwtToken(response.Email, "Admin", response.UserID, response.FullName);

            _logger.LogInformation("Admin login successful for email: {Email}", loginDTO.Email);

            return Ok(new
            {
                Token = token
            });
        }
    }
}
