using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<SystemAccount> GetUserByEmail(string email);
        Task<bool> GetUserCurrent(string email, short id);
        Task<SystemAccount?> GetUserByEmailAndPassword(string email, string password);
        Task<bool> GetUserByEmailAndPasswordAdmin(string email, string password);
        Task<SystemAccount?> GetUserById(int? id);
        Task<IList<SystemAccount>> GetListUser();
        Task<bool> IsEmailExistAsync(string email);
        Task<SystemAccount> CreateUser(SystemAccount account);
        Task<SystemAccount> UpdateUser(SystemAccount account);
        Task DeleteUser(SystemAccount account);
    }
}
