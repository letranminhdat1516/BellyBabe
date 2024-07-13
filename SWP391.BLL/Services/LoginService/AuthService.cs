using System.Threading.Tasks;
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


        public async Task<UserLoginResponseDTO> UserLoginAsync(UserLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(loginDTO.PhoneNumber);
            if (user == null || user.Otp != loginDTO.OTP || user.Otpexpiry < DateTime.UtcNow)
            {
                return null;
            }

            var token = GenerateJwtToken(user.Email, "User", user.UserId);
            return new UserLoginResponseDTO { Token = token, PhoneNumber = user.PhoneNumber };
        }

        public async Task<AdminLoginResponseDTO> AdminLoginAsync(AdminLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);
            if (user == null || user.Password != loginDTO.Password || (user.RoleId != 1 && user.RoleId != 2))
            {
                return null;
            }

            var isFirstLogin = user.IsFirstLogin;

            if (user.IsFirstLogin)
            {
                user.IsFirstLogin = false;
                await _userRepository.UpdateUserAsync(user);
            }

            var token = GenerateJwtToken(user.Email, "Admin", user.UserId);

            return new AdminLoginResponseDTO
            {
                UserID = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                RoleId = user.RoleId,
                Image = user.Image,
                IsFirstLogin = isFirstLogin,
                Token = token
            };
        }

        public string GenerateJwtToken(string email, string role, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
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

