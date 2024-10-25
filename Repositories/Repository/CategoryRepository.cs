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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementFall2024Context _context;

        public CategoryRepository(FunewsManagementFall2024Context context)
        {
            _context = context;
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            return await _context.Categories.Include(c => c.ParentCategory).ToListAsync();
        }
        public async Task<IList<Category>> GetAllCategoriesIsActive()
        {
            return await _context.Categories.Include(c => c.ParentCategory).Where(c => c.IsActive == true).ToListAsync();
        }
        public async Task<Category> GetCategoryById(short? categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        public async Task<bool> GetCategoryCurrent(string name, short id)
        {
            return await _context.Categories.AnyAsync(u => u.CategoryName == name && u.CategoryId != id);
        }
        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        }
        public async Task CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
