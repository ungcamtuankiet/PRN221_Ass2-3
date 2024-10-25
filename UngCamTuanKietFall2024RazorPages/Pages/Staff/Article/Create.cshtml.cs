using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Dtos.Article;
using Repositories.Entities;
using Services.IService;
using Services.Service;

namespace UngCamTuanKietFall2024RazorPages.Pages.Article
{
    public class CreateModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IAuthService _authService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public CreateModel(IArticleService articleService, ICategoryService categoryService, ITagService tagService, IAuthService authService, IHubContext<SignalRHub> hubContext)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _authService = authService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = await _authService.GetUserRole("StaffRole");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if(userRole == 1)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesIsActiveAsync(), "CategoryId", "CategoryName");
                ViewData["TagId"] = new SelectList(await _tagService.GetTagsAsync(), "TagId", "TagName");
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;
        public int? UserRole { get; private set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user_Id = HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            short userId = (short)user_Id;
            var result = await _articleService.CreateNewsArticleAsync(NewsArticle, userId);
            if(result.Code == 1)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToPage("/Staff/Article/Create");
            }
            TempData["SuccessMessage"] = result.Message;
            await _hubContext.Clients.All.SendAsync("RefreshData");
            return RedirectToPage("/Staff/StaffPage");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
