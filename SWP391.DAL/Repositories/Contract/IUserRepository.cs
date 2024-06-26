<<<<<<< HEAD
﻿using System.Threading.Tasks;
using SWP391.DAL.Entities;
=======
﻿using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
>>>>>>> master

namespace SWP391.DAL.Repositories.Contract
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
<<<<<<< HEAD
=======
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
>>>>>>> master
    }
}
