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
        Task<IList<Tag>> GetAll();
        Task<Tag?> GetTagByNameAsync(string tagName);
        Task<Tag?> GetTagIdAsync(int id);
        Task AddTagAsync(Tag tag);
    }
}
