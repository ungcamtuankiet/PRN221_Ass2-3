using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.Lecturer
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly IAuthService _authService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public IndexModel(IArticleService articleService, IAuthService authService, IHubContext<SignalRHub> hubContext)
        {
            _articleService = articleService;
            _authService = authService;
            _hubContext = hubContext;
        }
        public int? UserRole { get; private set; }
        public IList<NewsArticle> NewsArticle { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await _hubContext.Clients.All.SendAsync("RefreshData");
            NewsArticle = await _articleService.GetAllNewsArticlesAsync();
            UserRole = await _authService.GetUserRole("LecturerRole");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
