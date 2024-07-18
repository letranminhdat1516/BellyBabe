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

    [HttpPost("upload/{userId}")]
    public async Task<IActionResult> Upload(int userId, [FromBody] UserUploadProfile userUploadProfile)
    {
        if (userUploadProfile == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.UserName = userUploadProfile.UserName;
        user.PhoneNumber = NormalizePhoneNumber(userUploadProfile.PhoneNumber);
        user.Email = userUploadProfile.Email;
        user.Address = userUploadProfile.Address;
        user.FullName = userUploadProfile.FullName;
        user.Image = userUploadProfile.Image;

        var userUpdateDto = new UserUpdateDTO
        {
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Password = user.Password,
            Email = user.Email,
            Address = user.Address,
            FullName = user.FullName,
            RoleId = userUploadProfile.RoleId,
            Image = user.Image
        };

        var updatedUser = await _userService.UpdateUserAsync(userId, userUpdateDto);

        return Ok(updatedUser);
    }

    [HttpPut("update/{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDTO userUpdateDto)
    {
        if (userUpdateDto == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.UserName = userUpdateDto.UserName;
        user.PhoneNumber = NormalizePhoneNumber(userUpdateDto.PhoneNumber);
        user.Email = userUpdateDto.Email;
        user.Address = userUpdateDto.Address;
        user.FullName = userUpdateDto.FullName;
        user.Image = userUpdateDto.Image;

        var updatedUser = await _userService.UpdateUserAsync(userId, userUpdateDto);

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
    [HttpGet("contact-info/{userId}")]
    public async Task<IActionResult> GetUserContactInfo(int userId)
    {
        var contactInfo = await _userService.GetUserContactInfoAsync(userId);
        if (contactInfo == null)
        {
            return NotFound("User not found.");
        }
        return Ok(contactInfo);
    }

    [HttpPost("contact-info/{userId}")]
    public async Task<IActionResult> UpdateContactInfo(int userId, [FromBody] UserContactInfoDTO contact)
    {
        if (contact == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = await _userService.UpdateContactInfoAsync(userId, contact);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(new { message = "Contact info updated successfully." });
    }
    private string NormalizePhoneNumber(string phoneNumber)
    {
        if (phoneNumber.StartsWith("0"))
        {
            phoneNumber = "84" + phoneNumber.Substring(1);
        }
        return phoneNumber;
    }
}


