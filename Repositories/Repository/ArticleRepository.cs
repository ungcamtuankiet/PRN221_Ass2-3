using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly FunewsManagementFall2024Context _context;

        public ArticleRepository(FunewsManagementFall2024Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetArticleByUserId(int userId)
        {
            return await _context.NewsArticles.Where(a => a.CreatedById == userId).ToListAsync();
        }

        public async Task<IList<NewsArticle>> GetAllNewsArticles()
        {
            return await _context.NewsArticles.Where(a => a.NewsStatus == true)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .ToListAsync();
        }
        public async Task<IList<NewsArticle>> GetNewsArticleByUserId(short userId)
        {
            return await _context.NewsArticles.Where(a => a.CreatedById == userId)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .ToListAsync();
        }

        public async Task<NewsArticle> GetNewsArticleById(string articleId)
        {
            return await _context.NewsArticles
               .Include(n => n.Tags)
               .FirstOrDefaultAsync(n => n.NewsArticleId == articleId);
        }

        public async Task CreateNewsArticle(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsArticle(NewsArticle article)
        {
            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsArticle(NewsArticle article)
        {
            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
        }

    }
}
