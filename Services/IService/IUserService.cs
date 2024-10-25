
using Repositories.Dtos.Auth;
using Repositories.Dtos.User;
using Repositories.Entities;
using Repositories.Response;

namespace Services.IService
{
    public interface IUserService
    {
        Task<Response?> Login(LoginUserDto loginUserDto);
        Task<SystemAccount> GetUserByEmail(string email);
        Task<SystemAccount> GetUserById(int? userId);
        Task<IList<SystemAccount>> GetAllUsersAsync();
        Task<Response> CreateUserAsync(SystemAccount account);
        Task<Response> UpdateUserAsync(SystemAccount account);
        Task DeleteUserAsync(SystemAccount account);
    }
}
