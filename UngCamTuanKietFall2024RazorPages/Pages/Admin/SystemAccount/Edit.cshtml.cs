using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.SystemAccount
{
    public class EditModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public EditModel(IAuthService authService, IUserService userService)
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
                SystemAccount = systemaccount;
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login"); 
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Admin/SystemAccount/Create");
            }

            var result = await _userService.UpdateUserAsync(SystemAccount);
            if (result.Code == 1)
            {
                TempData["ErrorMessage"] = result.Message;
                return Page();
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
