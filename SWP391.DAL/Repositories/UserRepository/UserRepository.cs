using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.Contract;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly Swp391Context _context;

        public UserRepository(Swp391Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Logging if user is not found
                Console.WriteLine($"User with email {email} not found.");
            }
            else
            {
                // Logging if user is found
                Console.WriteLine($"User with email {email} found: {user.UserName}");
            }

            return user;
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task AddUserAsync(User user)
        {
            user.PhoneNumber = NormalizePhoneNumber(user.PhoneNumber);

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber || u.Email == user.Email);

            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.Address = user.Address;
                existingUser.FullName = user.FullName;
                existingUser.RoleId = user.RoleId;
                existingUser.Image = user.Image;

                _context.Users.Update(existingUser);
            }
            else
            {
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            user.PhoneNumber = NormalizePhoneNumber(user.PhoneNumber);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = "84" + phoneNumber.Substring(1);
            }
            return phoneNumber;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Blogs.RemoveRange(user.Blogs);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<int> userIds)
        {
            return await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
        }

        public async Task<User> GetUserByPhoneNumberAndRoleIdAsync(string phoneNumber, int roleId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.RoleId == roleId);
        }
        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
