using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.Admin
{
    public class AdminPageModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ICategoryService _categoryService;

        public AdminPageModel(IAuthService authService, ICategoryService categoryService)
        {
            _authService = authService;
            _categoryService = categoryService;
        }

        public int? UserRole { get; private set; }
        public IList<Repositories.Entities.Category> Category { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if(userRole != "Admin")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page";
                await _authService.ClearSession();
                return RedirectToPage("/Auth/Login");
            }
            UserRole = await _authService.GetUserRole("AdminRole");
            Category = await _categoryService.GetAllCategoriesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
