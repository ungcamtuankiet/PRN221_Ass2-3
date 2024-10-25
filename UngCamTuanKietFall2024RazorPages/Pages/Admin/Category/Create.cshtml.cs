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

namespace UngCamTuanKietFall2024RazorPages.Pages.Category
{
    public class CreateModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService, IAuthService authService)
        {

            _categoryService = categoryService;
            _authService = authService;
        }

        public async Task<IActionResult> OnGet()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if(userRole == "Admin")
            {
                ViewData["ParentCategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        [BindProperty]
        public Repositories.Entities.Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Admin/Category/Create");
            }
            var result = await _categoryService.CreateCategoryAsync(Category);
            if (result.Code == 1)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToPage("/Admin/Category/Create");
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToPage("/Admin/AdminPage");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
