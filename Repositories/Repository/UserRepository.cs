using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Data;
using Repositories.Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly FunewsManagementFall2024Context _context;

        public UserRepository(IConfiguration configuration, FunewsManagementFall2024Context context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<SystemAccount?> GetUserByEmailAndPassword(string email, string password)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(u => u.AccountEmail == email && u.AccountPassword == password);
        }
        public async Task<bool> GetUserByEmailAndPasswordAdmin(string email, string password)
        {
            var userName = _configuration["AdminAccount:UserName"];
            var passWord = _configuration["AdminAccount:Password"];
            if(userName == email && passWord == password)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> GetUserCurrent(string email, short id)
        {
            return await _context.SystemAccounts.AnyAsync(u => u.AccountEmail == email && u.AccountId != id);
        }
        public async Task<SystemAccount> GetUserByEmail(string email)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(u => u.AccountEmail == email);
        }
        public async Task<SystemAccount?> GetUserById(int? id)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(u => u.AccountId == id);
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.SystemAccounts.AnyAsync(a => a.AccountEmail == email);
        }
        public async Task<IList<SystemAccount>> GetListUser()
        {
            var _context = new FunewsManagementFall2024Context();
            return await _context.SystemAccounts.ToListAsync();
        }
        public async Task<SystemAccount> CreateUser(SystemAccount newUser)
        {
            var _context = new FunewsManagementFall2024Context();
            _context.SystemAccounts.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<SystemAccount> UpdateUser(SystemAccount systemAccount)
        {
            var _context = new FunewsManagementFall2024Context();
            _context.SystemAccounts.Update(systemAccount);
            await _context.SaveChangesAsync();
            return systemAccount;
        }

        public async Task DeleteUser(SystemAccount account)
        {
            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
