using Repositories.Dtos.Category;
using Repositories.Entities;
using Repositories.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetAllCategoriesAsync();
        Task<IList<Category>> GetAllCategoriesIsActiveAsync();
        Task<Category> GetCategoryById(short id);
        Task<Response> CreateCategoryAsync(Category category);
        Task<Response> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
