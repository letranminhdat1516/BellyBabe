using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Model.users;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            if (user == null)
            {
                return BadRequest("User creation failed.");
            }
            return Ok(user);
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userDto)
        {
            var user = await _userService.UpdateUserAsync(userDto);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}



