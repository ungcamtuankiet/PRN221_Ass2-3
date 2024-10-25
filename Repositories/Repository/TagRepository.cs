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

        // Lấy tất cả các tag
        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        // Tìm tag theo ID
        public async Task<Tag?> GetByIdAsync(int tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        // Thêm tag mới
        public async Task<Tag> AddTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        // Cập nhật tag
        public async Task<Tag> UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        // Xóa tag
        public async Task DeleteTagAsync(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
