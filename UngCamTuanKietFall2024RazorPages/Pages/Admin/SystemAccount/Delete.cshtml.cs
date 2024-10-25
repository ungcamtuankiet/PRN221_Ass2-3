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
    public class DeleteModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public DeleteModel(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [BindProperty]
        public Repositories.Entities.SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }
                var systemaccount = await _userService.GetUserById(id);

                if (systemaccount == null)
                {
                    return NotFound();
                }
                else
                {
                    SystemAccount = systemaccount;
                }
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemaccount = await _userService.GetUserById(id);
            if (systemaccount != null)
            {
                SystemAccount = systemaccount;
                await _userService.DeleteUserAsync(SystemAccount);
                TempData["SuccessMessage"] = "Delete user successfully";
                return RedirectToPage("/Admin/SystemAccount/Index");
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
