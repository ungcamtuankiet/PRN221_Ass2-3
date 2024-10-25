using Microsoft.AspNetCore.Http;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public async Task<int> GetUserRole(string userRole)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var user_Role =  session.GetInt32(userRole);
            return user_Role ?? -1;
        }
    }
}
