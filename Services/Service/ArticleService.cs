using Repositories.Dtos.Article;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.Response;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IList<NewsArticle>> GetAllNewsArticlesAsync()
        {
            return await _articleRepository.GetAllNewsArticles();
        }
        public async Task<IList<NewsArticle>> GetAllNewsArticlesByUserIdAsync(short userId)
        {
            return await _articleRepository.GetNewsArticleByUserId(userId);
        }

        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
        {
            return await _articleRepository.GetNewsArticleById(id);
        }

        public async Task<Response> CreateNewsArticleAsync(NewsArticle newsArticle, short? userId)
        {
            var getArticleById = await _articleRepository.GetNewsArticleById(newsArticle.NewsArticleId);
            if(getArticleById != null)
            {
                return new Response()
                {
                    Code = 1,
                    Message = "Article Id already exist",
                    Data = null
                };
            }
            if (newsArticle.Headline == null || newsArticle.NewsTitle == null || newsArticle.NewsContent == null
                || newsArticle.NewsSource == null || newsArticle.CategoryId == null)
            {
                return new Response()
                {
                    Code = 1,
                    Message = "Please enter full information",
                    Data = null
                }; 
            }
            if (newsArticle.CategoryId != null)
            {
                var category = await _categoryRepository.GetCategoryById(newsArticle.CategoryId.Value);
                if (category == null)
                {
                    return new Response()
                    {
                        Code = 1,
                        Message = "Category not found",
                        Data = null
                    };
                }
            }
            newsArticle.CreatedById = userId;
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = true;
            await _articleRepository.CreateNewsArticle(newsArticle);
            return new Response()
            {
                Code = 0,
                Message = "Create New Article Successfully",
                Data = newsArticle
            };
        }

        public async Task<Response> UpdateNewsArticleAsync(UpdateArticleDto update, short? userId)
        {
            // Tìm bài viết cần cập nhật
            var article = await _articleRepository.GetNewsArticleById(update.NewsTitle);

            if (article == null)
            {
                return new Response()
                {
                    Code = 1,
                    Message = "Article not found",
                    Data = null
                };
            }

            // Cập nhật các trường dữ liệu
            article.NewsTitle = update.NewsTitle;
            article.Headline = update.Headline;
            article.NewsContent = update.NewsContent;
            article.NewsSource = update.NewsSource;
            article.NewsStatus = update.NewsStatus;
            article.CategoryId = update.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.UpdatedById = userId;

            // Xử lý danh sách tag (nếu có)
            if (update.TagIds != null && update.TagIds.Any())
            {
                // Xóa các tags hiện tại
                article.Tags.Clear();

                // Thêm các tags mới từ update.TagIds
                foreach (var tagId in update.TagIds)
                {
                    var tag = await _tagRepository.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        article.Tags.Add(tag);
                    }
                }
            }

            // Lưu thay đổi
            await _articleRepository.UpdateNewsArticle(article);

            return new Response()
            {
                Code = 0,
                Message = "Update Article Successfully",
                Data = article
            };
        }


        public async Task<Response> DeleteNewsArticleAsync(NewsArticle newsArticle)
        {


            // Xóa bài viết
            await _articleRepository.DeleteNewsArticle(newsArticle);

            return new Response()
            {
                Code = 0,
                Message = "Delete Article Successfully",
                Data = null
            };
        }
    }
}
