using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Entities;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.Staff
{
    public class StaffPageModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IArticleService _articleService;

        public StaffPageModel(IAuthService authService, IArticleService articleService)
        {
            _authService = authService;
            _articleService = articleService;
        }
        public short UserId { get; private set; }
        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        public int? UserRole { get; private set; }

        public async Task OnGetAsync()
        {
            var userRole = HttpContext.Session.GetInt32("UserRole");
            UserRole = await _authService.GetUserRole("StaffRole");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userRole != 1)
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page";
                await _authService.ClearSession();
                RedirectToPage("/Auth/Login");
                
            }
            if (userId.HasValue)
            {
                UserId = (short)userId.Value;
                NewsArticle = await _articleService.GetAllNewsArticlesByUserIdAsync(UserId);
            }
            else
            {
                RedirectToPage("/Auth/Login");
            }
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
