using Repositories.Dtos.Article;
using Repositories.Entities;
using Repositories.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IArticleService
    {
        Task<IList<NewsArticle>> GetAllNewsArticlesAsync();
        Task<IList<NewsArticle>> GetAllNewsArticlesByUserIdAsync(short userId);
        Task<NewsArticle> GetNewsArticleByIdAsync(string id);
        Task<Response> CreateNewsArticleAsync(NewsArticle newsArticle, short? userId);
        Task<Response> UpdateNewsArticleAsync(UpdateArticleDto updateArticleDto, short? userId);
        Task<Response> DeleteNewsArticleAsync(NewsArticle newsArticle);
    }
}
