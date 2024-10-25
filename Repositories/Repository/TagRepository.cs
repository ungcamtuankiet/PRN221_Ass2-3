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
    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementFall2024Context _context;

        public TagRepository(FunewsManagementFall2024Context context)
        {
            _context = context;
        }
        public async Task<IList<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        // Lấy tất cả các tag
        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);
        }

        public async Task AddTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<Tag?> GetTagIdAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.TagId == id);
        }
    }
}
