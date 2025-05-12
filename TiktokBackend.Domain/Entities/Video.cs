using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Entities
{
    public class Video
    {
        public Guid Id { get; set; } = Guid.NewGuid();    
        public Guid UserId { get; set; }
        public string Type { get; set; } = string.Empty;

        public string ThumbUrl { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Music { get; set; } = string.Empty;

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int SharesCount { get; set; } = 0;
        public int ViewsCount { get; set; } = 0;

        public string Viewable { get; set; } = "public";  // public, private, friends...

        public string Allows { get; set; }  // ["comment", ...]

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }  
        public virtual VideoMeta Meta { get; set; } 
    }
}
