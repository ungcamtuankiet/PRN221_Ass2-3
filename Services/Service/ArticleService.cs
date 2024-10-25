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

        public async Task<Response> CreateNewsArticleAsync(NewsArticle newsArticle, IEnumerable<int> tags, short? userId)
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
            await AssignTagsToArticle(newsArticle, tags);
            await _articleRepository.CreateNewsArticle(newsArticle);
            return new Response()
            {
                Code = 0,
                Message = "Create New Article Successfully",
                Data = newsArticle
            };
        }

        public async Task<Response> UpdateNewsArticleAsync(UpdateArticleDto update, IEnumerable<int> tags, short? userId)
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

            // Lưu thay đổi
            await AssignTagsToArticle(article, tags);
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
        private async Task AssignTagsToArticle(NewsArticle newsArticle, IEnumerable<int> tags)
        {
            newsArticle.Tags.Clear();

            foreach (var tagId in tags)
            {
                var tag = await _tagRepository.GetTagIdAsync(tagId) ?? new Tag { TagId = tagId };
                if (tag.TagId == 0)
                {
                    await _tagRepository.AddTagAsync(tag);
                }
                newsArticle.Tags.Add(tag);
            }
        }
    }
}
