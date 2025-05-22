using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.DTOs
{
    public class VideoDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string ThumbUrl { get; set; }
        public string FileUrl { get; set; }

        public string Description { get; set; }
        public string Music { get; set; }

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int SharesCount { get; set; } = 0;
        public int ViewsCount { get; set; } = 0;

        public bool IsLiked { get; set; } = false;

        public string Viewable { get; set; } = "public";  // public, private, friends...

        public string[] Allows { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public UserDto User { get; set; }
        public VideoMetadata Meta { get; set; }
    }
}
