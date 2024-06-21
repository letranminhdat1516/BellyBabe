using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.Contract;
using SWP391.DAL.Swp391DbContext;
using System;
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
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
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
    }
}
