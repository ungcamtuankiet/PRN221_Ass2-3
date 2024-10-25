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

namespace UngCamTuanKietFall2024RazorPages.Pages.Category
{
    public class DetailsModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _authService;

        public DetailsModel(IAuthService authService, ICategoryService categoryService)
        {
            _authService = authService;
            _categoryService = categoryService;
        }

        public Repositories.Entities.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    Category = category;
                }
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
