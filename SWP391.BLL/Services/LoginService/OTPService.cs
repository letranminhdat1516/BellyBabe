using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Repositories.Contract;
using SWP391.DAL.Swp391DbContext;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OtpService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OtpService> _logger;
    private readonly IConfiguration _configuration;
    private readonly Swp391Context _context;
    public OtpService(IUserRepository userRepository, ILogger<OtpService> logger, IConfiguration configuration, Swp391Context context)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public string GenerateOtp()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var byteArray = new byte[2];
            rng.GetBytes(byteArray);
            var otp = BitConverter.ToUInt16(byteArray, 0) % 10000; // Generate a number between 0 and 9999
            var otpString = otp.ToString("D4");
            _logger.LogInformation($"Generated OTP: {otpString}");
            return otpString;
        }
    }

    public async Task SaveOtpAsync(string identifier, string otp, string userName, bool isEmail = false)
    {
        User user;
        if (isEmail)
        {
            user = await _userRepository.GetUserByEmailAsync(identifier);
        }
        else
        {
            user = await _userRepository.GetUserByPhoneNumberAsync(identifier);
        }

        if (user == null)
        {
            user = new User
            {
                UserName = userName,
                Otp = otp,
                Otpexpiry = DateTime.UtcNow.AddMinutes(5),
                Password = "default_password",
                RoleId = 3
            };
            if (isEmail)
            {
                user.Email = identifier;
            }
            else
            {
                user.PhoneNumber = identifier;
            }
            await _userRepository.AddUserAsync(user);
            _logger.LogInformation($"Added new user with identifier: {identifier} and OTP: {otp}");
        }
        else
        {
            user.Otp = otp;
            user.Otpexpiry = DateTime.UtcNow.AddMinutes(5);
            await _userRepository.UpdateUserAsync(user);
            _logger.LogInformation($"Updated user with identifier: {identifier} with new OTP: {otp}");
        }
    }


    public async Task SendOtpViaEmailAsync(string email, string otp)
    {
        var emailService = new EmailService(_configuration);
        await emailService.SendEmailAsync(email, "Your OTP Code", $"Your OTP code is {otp}");
    }

    public async Task SendOtpViaSmsAsync(string phoneNumber, string otp)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", _configuration["StringeeAuthToken"]);

            var body = new
            {
                from = new
                {
                    type = "external",
                    number = _configuration["StringeeNumber"],
                    alias = "STRINGEE_NUMBER"
                },
                to = new[]
                {
                    new
                    {
                        type = "external",
                        number = phoneNumber,
                        alias = "TO_NUMBER"
                    }
                },
                answer_url = _configuration["StringeeAnswerUrl"],
                actions = new[]
                {
                    new
                    {
                        action = "talk",
                        text = $"MÃ XÁC THỰC CỦA BẠN LÀ {otp}"
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.stringee.com/v1/call2/callout", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Sent OTP to {phoneNumber}. Response: {responseContent}");
        }
    }

    public async Task<bool> VerifyOtpAsync(string identifier, string otp, bool isEmail = false)
    {
        User user;
        if (isEmail)
        {
            user = await _userRepository.GetUserByEmailAsync(identifier);
        }
        else
        {
            user = await _userRepository.GetUserByPhoneNumberAsync(identifier);
        }

        var isValid = user != null && user.Otp == otp && user.Otpexpiry >= DateTime.UtcNow;
        _logger.LogInformation($"Verification result for identifier {identifier}: {isValid}");
        return isValid;
    }
    public async Task<string> GetOtpByPhoneNumberAsync(string phoneNumber)
    {
        var otpEntity = await _context.Users
                                      .Where(u => u.PhoneNumber == phoneNumber)
                                      .Select(u => u.Otp)
                                      .FirstOrDefaultAsync();
        return otpEntity;
    }


    public async Task<UserLoginResponseDTO> LoginAsync(UserLoginDTO loginDTO)
    {
        var isValid = await VerifyOtpAsync(loginDTO.PhoneNumber, loginDTO.OTP);
        if (!isValid)
        {
            return null;
        }

        // Kiểm tra xem số điện thoại
        var user = await _userRepository.GetUserByPhoneNumberAsync(loginDTO.PhoneNumber);

        // Nếu người dùng tồn tại thì đăng nhập ở hàm này
        if (user != null)
        {
            var token = GenerateJwtToken(user.PhoneNumber);
            return new UserLoginResponseDTO
            {
                Token = token,
                UserID = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                RoleId = user.RoleId,
                Image = user.Image,
                IsFirstLogin = user.IsFirstLogin
            };
        }

        // tạo mới người dùng khi đã đã có đăng nhập
        user = new User
        {
            PhoneNumber = loginDTO.PhoneNumber,
            Otp = loginDTO.OTP,
            Otpexpiry = DateTime.UtcNow.AddMinutes(5),
            Password = "default_password",
            RoleId = 3,
            IsFirstLogin = true
        };

        await _userRepository.AddUserAsync(user);

        var newToken = GenerateJwtToken(user.PhoneNumber);
        return new UserLoginResponseDTO
        {
            Token = newToken,
            UserID = user.UserId,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Password = user.Password,
            Email = user.Email,
            Address = user.Address,
            FullName = user.FullName,
            RoleId = user.RoleId,
            Image = user.Image,
            IsFirstLogin = user.IsFirstLogin
        };
    }



    private string GenerateJwtToken(string phoneNumber)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, phoneNumber),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
