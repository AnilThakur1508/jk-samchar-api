using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSamachar.DTO
{
    public class JkNewsDto
    {
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }
        public string? UserId { get; set; }
    }
}
