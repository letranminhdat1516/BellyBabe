using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Model.users;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromBody] UserUploadProfile userUploadProfile)
    {
        if (userUploadProfile == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = await _userService.GetUserByIdAsync(userUploadProfile.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.UserName = userUploadProfile.UserName;
        user.PhoneNumber = userUploadProfile.PhoneNumber;
        user.Email = userUploadProfile.Email;
        user.Address = userUploadProfile.Address;
        user.FullName = userUploadProfile.FullName;
        user.Image = userUploadProfile.Image;

        var updatedUser = await _userService.UpdateUserAsync(new UserUpdateDTO
        {
            UserId = user.UserId,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Password = user.Password, 
            Email = user.Email,
            Address = user.Address,
            FullName = user.FullName,
            RoleId = userUploadProfile.RoleId,
            Image = userUploadProfile.Image

        });

        return Ok(updatedUser);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }
    [HttpGet("{userId}/address")]
    public async Task<IActionResult> GetAddress(int userId)
    {
        var address = await _userService.GetAddressAsync(userId);
        if (address == null)
        {
            return NotFound("User not found.");
        }
        return Ok(new { Address = address });
    }

    [HttpPost("{userId}/address")]
    public async Task<IActionResult> UpdateAddress(int userId, [FromBody] UpdateAddressDTO model)
    {
        if (model == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = await _userService.UpdateAddressAsync(userId, model.Address);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(new { message = "Address updated successfully." });
    }
}

public class UpdateAddressDTO
{
    public string Address { get; set; }
}

