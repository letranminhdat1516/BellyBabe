using Microsoft.Extensions.Logging;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.users;
using SWP391.DAL.Repositories.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
namespace SWP391.BLL.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly OtpService otpService;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, EmailService emailService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
             _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
         
        }

        private object logger()
        {
            throw new NotImplementedException();
        }
        public async Task<string> GetAddressAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user?.Address;
        }
        public async Task<User> UpdateAddressAsync(int userId, string address)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Address = address;
                await _userRepository.UpdateUserAsync(user);
            }
            return user;
        }
        public async Task<User> CreateUserAsync(UserCreateDTO userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                PhoneNumber = userDto.PhoneNumber,
                Password = userDto.Password,
                Email = userDto.Email,
                Address = userDto.Address,
                FullName = userDto.FullName,
                RoleId = userDto.RoleId
            };

            await _userRepository.AddUserAsync(user);
            return user;
        }

        public async Task<User> UpdateUserAsync(UserUpdateDTO userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userDto.UserId);
            if (user != null)
            {
                user.UserName = userDto.UserName;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Password = userDto.Password; 
                user.Email = userDto.Email;
                user.Address = userDto.Address;
                user.FullName = userDto.FullName;
                user.Image = userDto.Image;


                if (userDto.RoleId.HasValue)
                {
                    user.RoleId = userDto.RoleId.Value;
                }

                await _userRepository.UpdateUserAsync(user);
            }
            return user;
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
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
    }
}
