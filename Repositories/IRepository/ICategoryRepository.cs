using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetAllCategories();
        Task<IList<Category>> GetAllCategoriesIsActive();
        Task<Category> GetCategoryById(short? categoryId);
        Task<Category> GetCategoryByName(string name);
        Task<bool> GetCategoryCurrent(string name, short id);
        Task CreateCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);
    }
}
