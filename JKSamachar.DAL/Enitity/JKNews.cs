using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSamachar.DAL.Enitity
{
    public class JKNews : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }
        public bool? IsAdminedAllowPublish { get; set; }
    }
}
