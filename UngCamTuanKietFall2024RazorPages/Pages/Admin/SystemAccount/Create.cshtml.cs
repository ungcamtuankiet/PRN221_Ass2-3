using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.Data;
using Repositories.Entities;
using Services.IService;
using Services.Service;

namespace UngCamTuanKietFall2024RazorPages.Pages.SystemAccount
{
    public class CreateModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public CreateModel(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        [BindProperty]
        public Repositories.Entities.SystemAccount SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Admin/SystemAccount/Create");
            }
            var result = await _userService.CreateUserAsync(SystemAccount);
            if(result.Code == 1)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToPage("/Admin/SystemAccount/Create");
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToPage("/Admin/SystemAccount/Index");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
