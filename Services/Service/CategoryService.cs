using Repositories.Dtos.Category;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Repositories.Response;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategories();
        }
        public async Task<IList<Category>> GetAllCategoriesIsActiveAsync()
        {
            return await _categoryRepository.GetAllCategoriesIsActive();
        }

        public async Task<Category> GetCategoryById(short id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }
        public async Task<Response> CreateCategoryAsync(Category category)
        {
            if(category.CategoryName == null)
            {
                return new Response()
                {
                    Code = 1,
                    Message = "Category Name can not null",
                    Data = null
                };
            }
            var getCategoryName = await _categoryRepository.GetCategoryByName(category.CategoryName);
            if(getCategoryName != null)
            {
                return new Response() { Code = 1, Message = "Category Name already exist", Data = null };
            }
            category.IsActive = true;
            await _categoryRepository.CreateCategory(category);
            return new Response()
            {
                Code = 0,
                Message = "Create New Category Successfully",
                Data = category
            };
        }

        public async Task<Response> UpdateCategoryAsync(Category category)
        {
            var checkCategoryName = await _categoryRepository.GetCategoryCurrent(category.CategoryName, category.CategoryId);
            if (checkCategoryName) return new Response() { Code = 1, Message = "Category Name Alredy exist", Data = null };
            await _categoryRepository.UpdateCategory(category);
            return new Response()
            {
                Code = 0,
                Message = "Update Category Successfully",
                Data = category
            }; 
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await _categoryRepository.DeleteCategory(category);
        }
    }
}
