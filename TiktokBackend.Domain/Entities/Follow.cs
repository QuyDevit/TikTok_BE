using System.ComponentModel.DataAnnotations;

namespace TiktokBackend.Domain.Entities
{
    public class Follow
    {
        public Guid FollowerId { get; set; }
        public virtual User Follower { get; set; }

        public Guid FolloweeId { get; set; }
        public virtual User Followee { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.Now;
    }

}
