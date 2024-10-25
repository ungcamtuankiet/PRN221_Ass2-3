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
using Services.Service;

namespace UngCamTuanKietFall2024RazorPages.Pages.Article
{
    public class DeleteModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly IAuthService _authService;

        public DeleteModel(IArticleService articleService, IAuthService authService)
        {
            _articleService = articleService;
            _authService = authService;
        }
        public NewsArticle NewsArticle { get; set; }
        public int? UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var userRole = HttpContext.Session.GetInt32("UserRole");
            UserRole = await _authService.GetUserRole("StaffRole");
            if(userRole == 1)
            {
                NewsArticle = await _articleService.GetNewsArticleByIdAsync(id);
                if (NewsArticle == null)
                {
                    return NotFound();
                }
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = await _articleService.GetNewsArticleByIdAsync(id);
            if (newsarticle != null)
            {
                var result = await _articleService.DeleteNewsArticleAsync(newsarticle);
                if (result.Code == 1)
                {
                    TempData["ErrorMessage"] = result.Message;
                    return Page();
                }
                TempData["SuccessMessage"] = result.Message;
                return RedirectToPage("/Staff/StaffPage");
            }

            return RedirectToPage("/Staff/StaffPage");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
