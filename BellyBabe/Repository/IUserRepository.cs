using BellyBabe.Entity;
using System.Threading.Tasks;

namespace BellyBabe.Repository
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByPhoneNumberAsync(string phoneNumber);
        Task AddUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel user);
    }
}
