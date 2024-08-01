using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Login;
using SWP391.DAL.Model.users;
using SWP391.DAL.Repositories.BlogRepository;
using SWP391.DAL.Repositories.Contract;
using SWP391.DAL.Repositories.VoucherRepository;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
namespace SWP391.BLL.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly OtpService otpService;
        private readonly ILogger<UserService> _logger;
        private readonly VoucherRepository _voucherRepository;
        private readonly BlogRepository _blogRepository;

        public UserService(IUserRepository userRepository, EmailService emailService, VoucherRepository voucherRepository, BlogRepository blogRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
             _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _voucherRepository = voucherRepository ?? throw new ArgumentNullException(nameof(voucherRepository));
            _blogRepository = blogRepository ?? throw new ArgumentNullException(nameof(blogRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private object logger()
        {
            throw new NotImplementedException();
        }
        public async Task<User> LoginAsync(string phoneNumber, string password)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            if (user == null || user.Password != password)
            {
                return null;
            }

            return user;
        }
        public async Task<User> RegisterAsync(UserRegistrationModel registrationModel)
        {
            var existingUser = await _userRepository.GetUserByPhoneNumberAsync(registrationModel.PhoneNumber);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this phone number already exists.");
            }

            var newUser = new User
            {
                UserName = registrationModel.UserName,
                PhoneNumber = registrationModel.PhoneNumber,
                Password = registrationModel.Password,
                Email = registrationModel.Email,
                FullName = registrationModel.FullName,
                Address = registrationModel.Address,
                Image = registrationModel.Image,
                RoleId = 3
            };

            await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }
        public async Task<UserLoginResponseDTO> UserLoginAsync(UserLoginModel loginModel)

        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(loginModel.PhoneNumber);
            if (user == null || user.Password != loginModel.Password)
            {
                return null;
            }
            if (user.IsActive == false)
            {
                return null;
            }
            var token = GenerateJwtToken(user.PhoneNumber, "User", user.UserId, user.FullName);
            return new UserLoginResponseDTO { Token = token, PhoneNumber = user.PhoneNumber, FullName = user.FullName};

        }
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {

            return await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            
            return enteredPassword == storedPassword; 
        }
        public async Task<User> CompleteFirstLoginAsync(DAL.Model.Login.FirstTimeUserInfoModel model)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(model.PhoneNumber);
            if (user == null)
            {
                return null;
            }

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.Email = model.Email;
            user.IsFirstLogin = false;

            await _userRepository.UpdateUserAsync(user);
            return user;
        }
        public async Task<UserContactInfoDTO> GetUserContactInfoAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserContactInfoDTO
            {
                Address = user.Address,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<User> UpdateContactInfoAsync(int userId, UserContactInfoDTO contact)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.Address = contact.Address;
            user.PhoneNumber = contact.PhoneNumber;
            user.FullName = contact.FullName;

            await _userRepository.UpdateUserAsync(user);
            return user;
        }

        public async Task<User> CreateUserAsync(UserCreateDTO userDto)
        {
            // check role id = 3
            var existingUser = await _userRepository.GetUserByPhoneNumberAndRoleIdAsync(userDto.PhoneNumber, 3);
            if (existingUser != null)
            {
                
                throw new InvalidOperationException("Cannot create user with RoleId 1, 2, or 4 when phone number is already in use by a user with RoleId 3.");
            }

            var user = new User
            {
                UserName = userDto.UserName,
                PhoneNumber = userDto.PhoneNumber,
                Password = userDto.Password,
                Email = userDto.Email,
                Address = userDto.Address,
                FullName = userDto.FullName,
                RoleId = userDto.RoleId,
                Image = userDto.Image
            };

            await _userRepository.AddUserAsync(user);
            return user;
        }


        public async Task<User> UpdateUserAsync(int userId, UserUpdateDTO userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.UserName = userDto.UserName;
                user.PhoneNumber = NormalizePhoneNumber(userDto.PhoneNumber);
                user.Password = userDto.Password; 
                user.Email = userDto.Email;
                user.Address = userDto.Address;
                user.FullName = userDto.FullName;
                user.Image = userDto.Image;
                user.IsActive = userDto.IsActive;

                if (userDto.RoleId.HasValue)
                {
                    user.RoleId = userDto.RoleId.Value;
                }

                await _userRepository.UpdateUserAsync(user);
            }
            return user;
        }
        public async Task<bool> BanUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.IsActive = false;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
        private string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber), "Phone number cannot be null or empty.");
            }

            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = "84" + phoneNumber.Substring(1);
            }

            return phoneNumber;
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false; // User không tồn tại
            }

            if (user.RoleId == 1)
            {
                throw new InvalidOperationException("Cannot delete admin users.");
            }

            var blogs = await _blogRepository.GetBlogsByUserIdAsync(userId);
            foreach (var blog in blogs)
            {
                await _blogRepository.DeleteBlog(blog.BlogId);
            }

            _userRepository.DeleteUser(user);

            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email, OtpService otpService)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            // Sinh mã OTP
            var otp = otpService.GenerateOtp();
            user.Otp = otp;
            user.Otpexpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);

            // Gửi OTP qua email
            await _emailService.SendEmailAsync(email, "Password Reset", $"Your OTP is {otp}");

            return otp;
        }

        public async Task<bool> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.Otp != otp || user.Otpexpiry < DateTime.UtcNow)
            {
                return false;
            }

            user.Password = newPassword;
            user.Otp = null;
            user.Otpexpiry = null;
            await _userRepository.UpdateUserAsync(user);

            return true;
        }
        public async Task<bool> SendVoucherToUsersAsync(List<int> userIds, string voucherCode)
        {
            var users = await _userRepository.GetUsersByIdsAsync(userIds);

            if (users == null || !users.Any())
            {
                return false;
            }
            bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
            var subject = "Voucher Belly and Babe";
            var message = $"Your voucher code is: {voucherCode}";

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.Email) && IsValidEmail(user.Email))
                {
                    await _emailService.SendEmailAsync(user.Email, subject, message);
                }
            }

            return true;
        }

        public async Task UpdateUserAsync(UserUpdateDTO userDto)
        {
            throw new NotImplementedException();
        }
        public string GenerateJwtToken(string identifier, string role, int userId,string fullname)
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
        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            return await _userRepository.CheckPhoneNumberExistsAsync(phoneNumber);
        }

    }
}
