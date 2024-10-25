using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Dtos.Article
{
    public class UpdateArticleDto
    {
        public string? NewsTitle { get; set; }
        public short? CategoryId { get; set; }
        public string Headline { get; set; } = null!;

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
