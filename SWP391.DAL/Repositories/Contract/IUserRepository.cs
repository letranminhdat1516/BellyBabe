using System.Threading.Tasks;
using SWP391.DAL.Entities;

namespace SWP391.DAL.Repositories.Contract
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
