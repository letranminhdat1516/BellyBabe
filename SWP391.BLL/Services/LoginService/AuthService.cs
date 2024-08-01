using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Repositories.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SWP391.BLL.Services.LoginService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<UserLoginResponseDTO> UserLoginAsync(UserLoginModel loginModel)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(loginModel.PhoneNumber);
            if (user == null || user.Password != loginModel.Password)
            {
                return null;
            }
         
            var token = GenerateJwtToken(user.PhoneNumber, "User", user.UserId, user.FullName);
            return new UserLoginResponseDTO
            {
                Token = token,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                UserID = user.UserId,
                RoleId = user.RoleId
            };
        }



        public async Task<AdminLoginResponseDTO> AdminLoginAsync(AdminLoginDTO loginDTO)
        {
            _logger.LogInformation("Attempting to login admin with email: {Email}", loginDTO.Email);

            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("No user found with email: {Email}", loginDTO.Email);
                return null;
            }

            _logger.LogInformation("User found: {UserName}, checking password", user.UserName);

            if (user.Password != loginDTO.Password)
            {
                _logger.LogWarning("Invalid password for email: {Email}", loginDTO.Email);
                return null;
            }

            _logger.LogInformation("Password is correct, checking role");

            if (user.RoleId != 1 && user.RoleId != 2 && user.RoleId != 4)
            {
                _logger.LogWarning("Unauthorized role for email: {Email}, RoleId: {RoleId}", loginDTO.Email, user.RoleId);
                return null;
            }

            var isFirstLogin = user.IsFirstLogin;

            _logger.LogInformation("IsFirstLogin status: {IsFirstLogin}", isFirstLogin);

            if (user.IsFirstLogin == true)
            {
                user.IsFirstLogin = false;
                await _userRepository.UpdateUserAsync(user);
            }

            var token = GenerateJwtToken(user.Email, "Admin", user.UserId, user.FullName);

            _logger.LogInformation("Admin login successful for email: {Email}", loginDTO.Email);

            return new AdminLoginResponseDTO
            {
                Token = token,
                UserID = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                RoleId = user.RoleId,
                Image = user.Image,
                IsFirstLogin = isFirstLogin
            };
        }



        public string GenerateJwtToken(string identifier, string role, int userId, string fullname)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, identifier),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("fullName", fullname ?? string.Empty) 
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
