using SWP391.DAL.Model.Login;

public interface IAuthService
{
    Task<AdminLoginResponseDTO> AdminLoginAsync(AdminLoginDTO loginDTO);
    Task<UserLoginResponseDTO> UserLoginAsync(UserLoginDTO loginDTO);
    string GenerateJwtToken(string email, string role);
}
