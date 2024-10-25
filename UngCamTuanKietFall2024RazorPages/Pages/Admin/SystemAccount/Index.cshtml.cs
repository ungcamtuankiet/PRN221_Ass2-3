using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.SystemAccount
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public IndexModel(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        public IList<Repositories.Entities.SystemAccount> SystemAccount { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                SystemAccount = await _userService.GetAllUsersAsync();
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
