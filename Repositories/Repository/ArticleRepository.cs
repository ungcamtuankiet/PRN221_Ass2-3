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
            var _context = new FunewsManagementFall2024Context();
            return await _context.NewsArticles.FirstOrDefaultAsync(a => a.NewsArticleId == articleId);
        }

        public async Task<NewsArticle> CreateNewsArticle(NewsArticle article)
        {
            await _context.NewsArticles.AddAsync(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<NewsArticle> UpdateNewsArticle(NewsArticle article)
        {
            _context.Entry<NewsArticle>(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task DeleteNewsArticle(NewsArticle article)
        {
            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}
