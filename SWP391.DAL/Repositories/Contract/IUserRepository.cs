using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.Contract
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        void DeleteUser(User user);
        Task SaveChangesAsync();
        Task<User> GetUserByNameAsync(string userName);
        Task<List<User>> GetUsersByIdsAsync(List<int> userIds);
    }
}
