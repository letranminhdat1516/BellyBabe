using System.Threading.Tasks;
using SWP391.DAL.Model.Login;

namespace SWP391.BLL.Services.LoginService
{
    public interface IAuthService
    {
        Task<AdminLoginResponseDTO> AdminLoginAsync(AdminLoginDTO loginDTO);
        Task<UserLoginResponseDTO> UserLoginAsync(UserLoginDTO loginDTO);
        string GenerateJwtToken(string email, string role, int userId);
    }
}
