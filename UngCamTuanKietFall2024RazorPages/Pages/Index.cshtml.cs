using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        public string? UserRole { get; private set; }

        public void OnGet()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
