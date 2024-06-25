using SWP391.DAL.Entities;
using SWP391.DAL.Model.users;
using SWP391.DAL.Repositories.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
namespace SWP391.BLL.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
            var user = await _userRepository.GetUserByPhoneNumberAsync(userDto.PhoneNumber);
            if (user != null)
            {
                user.UserName = userDto.UserName;
                user.Password = userDto.Password;
                user.Email = userDto.Email;
                user.Address = userDto.Address;
                user.FullName = userDto.FullName;
                user.RoleId = userDto.RoleId;

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
    }
}
