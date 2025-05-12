using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Entities
{
    public class CommentLike
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
