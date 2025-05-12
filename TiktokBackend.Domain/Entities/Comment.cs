using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid VideoId { get; set; }    
        public Guid UserId { get; set; }     
        public string Content { get; set; }

        public Guid? ParentCommentId { get; set; } 

        public int LikesCount { get; set; } = 0;  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
