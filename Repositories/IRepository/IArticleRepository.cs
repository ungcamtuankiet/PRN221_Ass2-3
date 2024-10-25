using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IArticleRepository
    {
        Task<IList<NewsArticle>> GetAllNewsArticles();
        Task<NewsArticle> GetNewsArticleById(string articleId);
        Task<IList<NewsArticle>> GetNewsArticleByUserId(short userId);
        Task CreateNewsArticle(NewsArticle article);
        Task UpdateNewsArticle(NewsArticle article);
        Task DeleteNewsArticle(NewsArticle article);
    }
}
