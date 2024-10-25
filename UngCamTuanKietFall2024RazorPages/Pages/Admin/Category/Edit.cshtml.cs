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

namespace UngCamTuanKietFall2024RazorPages.Pages.Category
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _authService;

        public EditModel(ICategoryService categoryService, IAuthService authService)
        {
            _categoryService = categoryService;
            _authService = authService;
        }

        [BindProperty]
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
                Category = category;
                ViewData["ParentCategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
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
                return Page();
            }

            var result = await _categoryService.UpdateCategoryAsync(Category);
            if (result.Code == 1)
            {
                ViewData["ParentCategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
                TempData["ErrorMessage"] = result.Message;
                return Page();
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
