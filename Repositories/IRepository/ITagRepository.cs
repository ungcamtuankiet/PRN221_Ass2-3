using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int tagId);
        Task<Tag> AddTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int tagId);
    }
}
