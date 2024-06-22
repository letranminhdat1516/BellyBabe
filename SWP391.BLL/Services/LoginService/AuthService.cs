using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Repositories.Contract;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWP391.BLL.Services.LoginService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AdminLoginResponseDTO> AdminLoginAsync(AdminLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return null;
            }

            if (user.Password != loginDTO.Password)
            {
                Console.WriteLine("Password mismatch.");
                return null;
            }

            if (user.Role == null)
            {
                Console.WriteLine("User role is null.");
                return null;
            }

            if (user.Role.RoleName != "Admin")
            {
                Console.WriteLine($"User role is not Admin, it is {user.Role.RoleName}.");
                return null;
            }

            return new AdminLoginResponseDTO { Email = user.Email };
        }

        public async Task<UserLoginResponseDTO> UserLoginAsync(UserLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(loginDTO.PhoneNumber);
            if (user == null || user.Otp != loginDTO.OTP || user.Otpexpiry < DateTime.UtcNow)
            {
                return null;
            }

            var token = GenerateJwtToken(user.Email, "User");
            return new UserLoginResponseDTO { Token = token, PhoneNumber = user.PhoneNumber };
        }

        public string GenerateJwtToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
