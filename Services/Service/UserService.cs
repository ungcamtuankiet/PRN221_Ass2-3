using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Repositories.Dtos.User;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.Response;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Repositories.Dtos.Auth;
using Microsoft.Extensions.Configuration;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<IList<SystemAccount>> GetAllUsersAsync()
        {
            return await _userRepository.GetListUser();
        }
        public async Task<SystemAccount> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<SystemAccount> GetUserById(int? userId)
        {
            return await _userRepository.GetUserById(userId);
        }
        public async Task<Response?> Login(LoginUserDto loginUserDto)
        {
            var loginAdmin = await _userRepository.GetUserByEmailAndPasswordAdmin(loginUserDto.Email, loginUserDto.Password);
            var login = await _userRepository.GetUserByEmailAndPassword(loginUserDto.Email, loginUserDto.Password);
            if (loginUserDto.Email == null || loginUserDto.Password == null)
            {
                return new Response() { Code = 1, Message = "Email and password cannot be blank", Data = null };
            }
            if (loginAdmin == true)
            {
                return new Response() { Code = 2, Message = "Login Successfully", Data = null };
            }
            if (login == null)
            {
                return new Response() { Code = 1, Message = "Email or Password incorrect", Data = null };
            }
            return new Response() { Code = 0, Message = "Login Successfully", Data = null };

        }
        public async Task<Response> CreateUserAsync(SystemAccount account)
        {
            if(account.AccountName == null || account.AccountEmail == null || account.AccountPassword == null || account.AccountRole == null)
            {
                return new Response() { Code = 1, Message = "Please fill all information", Data = null };
            }
            var getAccountEmail = await _userRepository.GetUserByEmail(account.AccountEmail);
            var getAccountId = await _userRepository.GetUserById(account.AccountId);
            if(account.AccountId <= 0)
            {
                return new Response() { Code = 1, Message = "AccountId must be more than 0", Data = null };
            }
            if(getAccountId != null)
            {
                return new Response() { Code = 1, Message = "AccountId already exist", Data = null };
            }
            if(getAccountEmail != null)
            {
                return new Response() { Code = 1, Message = "Email already exist", Data = null };
            }
            await _userRepository.CreateUser(account);
            return new Response() { Code = 0, Message = "Create new account successfully", Data = account };
        }
        public async Task<Response> UpdateUserAsync(SystemAccount account)
        {
            if (account.AccountName == null || account.AccountEmail == null || account.AccountPassword == null || account.AccountRole == null)
            {
                return new Response() { Code = 1, Message = "Please fill all information", Data = null };
            }
            bool getAccountEmail = await _userRepository.GetUserCurrent(account.AccountEmail, account.AccountId);
            if (getAccountEmail)
            {
                return new Response() { Code = 1, Message = "Email already exist", Data = null };
            }
            await _userRepository.UpdateUser(account);
            return new Response() { Code = 0, Message = "Update account successfully", Data = account };
        }
        public async Task DeleteUserAsync(SystemAccount account)
        {
            await _userRepository.DeleteUser(account);
        }
    }
}
