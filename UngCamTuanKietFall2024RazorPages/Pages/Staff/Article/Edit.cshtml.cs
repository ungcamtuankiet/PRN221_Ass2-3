using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Dtos.Article;
using Repositories.Entities;
using Services.IService;
using Services.Service;

namespace UngCamTuanKietFall2024RazorPages.Pages.Article
{
    public class EditModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService; 
        private readonly IAuthService _authService;

        [BindProperty]
        public UpdateArticleDto Article { get; set; }
        public Repositories.Entities.Category Category { get; set; } = default!;
        public Repositories.Entities.Tag Tags { get; set; } = default!;
        public int? UserRole { get; private set; }

        public EditModel(IArticleService articleService, ICategoryService categoryService, ITagService tagService, IAuthService authService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var userRole = HttpContext.Session.GetInt32("UserRole");
            UserRole = await _authService.GetUserRole("StaffRole");
            var article = await _articleService.GetNewsArticleByIdAsync(id);
            if(userRole == 1)
            {
                if (article == null)
                {
                    return NotFound();
                }

                Article = new UpdateArticleDto
                {
                    NewsTitle = article.NewsTitle,
                    Headline = article.Headline,
                    NewsContent = article.NewsContent,
                    CategoryId = article.CategoryId,
                    NewsSource = article.NewsSource,
                    NewsStatus = article.NewsStatus,
                    TagIds = article.Tags.Select(t => t.TagId).ToList()
                };

                ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesIsActiveAsync(), "CategoryId", "CategoryName");
                ViewData["TagId"] = new SelectList(await _tagService.GetTagsAsync(), "TagId", "TagName");
                return Page();
            }
            TempData["ErrorMessage"] = "You don't have permission to access this page";
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            short? userId = string.IsNullOrEmpty(userIdString) ? (short?)null : short.Parse(userIdString);
            var result = await _articleService.UpdateNewsArticleAsync(Article, userId);
            if(result.Code == 1)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToPage("/Staff/Article/Edit");
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToPage("/Staff/StaffPage");
        }
        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.ClearSession();
            return RedirectToPage("/Auth/Login");
        }
    }
}
