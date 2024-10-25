﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IAuthService
    {
        Task<int> GetUserRole(string userRole);
        Task ClearSession();
    }
}
