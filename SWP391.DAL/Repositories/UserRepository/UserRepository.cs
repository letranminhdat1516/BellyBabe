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
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task AddUserAsync(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (existingUser != null)
            {
                throw new InvalidOperationException("UserName already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<List<User>> GetUsersByIdsAsync(List<int> userIds)
        {
            return await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
        }
    }
}
