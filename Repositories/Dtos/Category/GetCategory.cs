using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Dtos.Category
{
    public class GetCategory
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }

    }
}
